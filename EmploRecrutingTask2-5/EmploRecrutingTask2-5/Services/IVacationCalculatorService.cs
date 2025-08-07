using EmploRecrutingTask2_5.Models;

namespace EmploRecrutingTask2_5.Services;

public interface IVacationCalculatorService
{
    int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
    Task<int> CountFreeDaysForEmployeeAsync(int employeeId);
    bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
    Task<bool> IfEmployeeCanRequestVacationAsync(int employeeId);
}
