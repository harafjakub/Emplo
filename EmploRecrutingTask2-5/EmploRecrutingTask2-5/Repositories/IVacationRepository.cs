using EmploRecrutingTask2_5.DTOs;
using EmploRecrutingTask2_5.Models;

namespace EmploRecrutingTask2_5.Repositories;

public interface IVacationRepository
{
    Task<List<Employee>> GetDotNetEmployeesWithVacationsInYearAsync(int year);
    Task<List<EmployeeVacationSummaryDto>> GetEmployeesWithUsedVacationDaysAsync(int year);
    Task<List<Team>> GetTeamsWithoutVacationsInYearAsync(int year);
}
