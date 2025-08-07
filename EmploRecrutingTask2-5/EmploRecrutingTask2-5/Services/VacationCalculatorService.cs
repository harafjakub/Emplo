using EmploRecrutingTask2_5.Exceptions;
using EmploRecrutingTask2_5.Models;
using EmploRecrutingTask2_5.Repositories;
using Microsoft.Extensions.Logging;

namespace EmploRecrutingTask2_5.Services;

public class VacationCalculatorService : IVacationCalculatorService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<VacationCalculatorService> _logger;

    public VacationCalculatorService(IEmployeeRepository employeeRepository, ILogger<VacationCalculatorService> logger)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (vacations == null) throw new ArgumentNullException(nameof(vacations));
        if (vacationPackage == null) throw new ArgumentNullException(nameof(vacationPackage));

        var currentYear = DateTime.Now.Year;

        if (vacationPackage.Year != currentYear)
        {
            _logger.LogWarning("Vacation package year {PackageYear} doesn't match current year {CurrentYear} for employee {EmployeeId}", 
                vacationPackage.Year, currentYear, employee.Id);
            return 0;
        }

        var vacationsInCurrentYear = vacations
            .Where(v => v.EmployeeId == employee.Id)
            .Where(v => 
                (v.DateSince.Year == currentYear) || 
                (v.DateUntil.Year == currentYear))
            .ToList();

        int usedVacationDays = 0;
        foreach (var vacation in vacationsInCurrentYear)
        {
            DateTime startDate = vacation.DateSince.Year < currentYear 
                ? new DateTime(currentYear, 1, 1) 
                : vacation.DateSince;

            DateTime endDate = vacation.DateUntil.Year > currentYear 
                ? new DateTime(currentYear, 12, 31) 
                : vacation.DateUntil;

            int days = (int)(endDate - startDate).TotalDays + 1;
            usedVacationDays += days;
        }

        int remainingDays = vacationPackage.GrantedDays - usedVacationDays;
        return remainingDays < 0 ? 0 : remainingDays;
    }

    public async Task<int> CountFreeDaysForEmployeeAsync(int employeeId)
    {
        try
        {
            _logger.LogInformation("Calculating remaining vacation days for employee {EmployeeId}", employeeId);

            var currentYear = DateTime.Now.Year;

            var employee = await _employeeRepository.GetEmployeeWithVacationPackageAsync(employeeId);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", employeeId);
                throw new BusinessLogicException($"Employee with ID {employeeId} not found");
            }

            var vacations = await _employeeRepository.GetEmployeeVacationsAsync(employeeId);

            var result = CountFreeDaysForEmployee(employee, vacations, employee.VacationPackage);

            _logger.LogInformation("Employee {EmployeeId} has {RemainingDays} vacation days remaining for {Year}", 
                employeeId, result, currentYear);

            return result;
        }
        catch (Exception ex) when (ex is not BusinessLogicException)
        {
            _logger.LogError(ex, "Unexpected error while calculating remaining vacation days for employee {EmployeeId}", employeeId);
            throw new BusinessLogicException($"Failed to calculate remaining vacation days for employee {employeeId}", ex);
        }
    }

    public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        if (vacations == null) throw new ArgumentNullException(nameof(vacations));
        if (vacationPackage == null) throw new ArgumentNullException(nameof(vacationPackage));

        var currentYear = DateTime.Now.Year;

        if (vacationPackage.Year != currentYear)
        {
            _logger.LogWarning("Vacation package year {PackageYear} doesn't match current year {CurrentYear} for employee {EmployeeId}", 
                vacationPackage.Year, currentYear, employee.Id);
            return false;
        }

        int remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);

        return remainingDays > 0;
    }

    public async Task<bool> IfEmployeeCanRequestVacationAsync(int employeeId)
    {
        try
        {
            _logger.LogInformation("Checking if employee {EmployeeId} can request vacation", employeeId);

            var currentYear = DateTime.Now.Year;

            var employee = await _employeeRepository.GetEmployeeWithVacationPackageAsync(employeeId);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", employeeId);
                throw new BusinessLogicException($"Employee with ID {employeeId} not found");
            }

            var vacations = await _employeeRepository.GetEmployeeVacationsAsync(employeeId);

            var result = IfEmployeeCanRequestVacation(employee, vacations, employee.VacationPackage);

            _logger.LogInformation("Employee {EmployeeId} can request vacation: {CanRequest}", 
                employeeId, result);

            return result;
        }
        catch (Exception ex) when (ex is not BusinessLogicException)
        {
            _logger.LogError(ex, "Unexpected error while checking if employee {EmployeeId} can request vacation", employeeId);
            throw new BusinessLogicException($"Failed to check if employee {employeeId} can request vacation", ex);
        }
    }
}
