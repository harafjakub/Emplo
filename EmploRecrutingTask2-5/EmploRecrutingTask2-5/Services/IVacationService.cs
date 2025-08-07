using EmploRecrutingTask2_5.DTOs;

namespace EmploRecrutingTask2_5.Services;

public interface IVacationService
{
    Task<List<EmployeeWithTeamDto>> GetDotNetEmployeesWithVacationsIn2019Async();
    Task<List<EmployeeVacationSummaryDto>> GetEmployeesWithUsedVacationDaysAsync();
    Task<List<TeamDto>> GetTeamsWithoutVacationsIn2019Async();
}
