using EmploRecrutingTask2_5.DTOs;
using EmploRecrutingTask2_5.Exceptions;
using EmploRecrutingTask2_5.Repositories;
using Microsoft.Extensions.Logging;

namespace EmploRecrutingTask2_5.Services;
public class VacationService : IVacationService
{
    private readonly IVacationRepository _repository;
    private readonly ILogger<VacationService> _logger;

    public VacationService(IVacationRepository repository, ILogger<VacationService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<EmployeeWithTeamDto>> GetDotNetEmployeesWithVacationsIn2019Async()
    {
        try
        {
            _logger.LogInformation("Retrieving .NET employees with vacations in 2019");

            var employees = await _repository.GetDotNetEmployeesWithVacationsInYearAsync(2019);

            var result = employees.Select(e => new EmployeeWithTeamDto
            {
                Id = e.Id,
                Name = e.Name,
                TeamName = e.Team.Name
            }).ToList();

            _logger.LogInformation("Found {Count} .NET employees with vacations in 2019", result.Count);
            return result;
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving .NET employees with vacations in 2019");
            throw new BusinessLogicException("Failed to retrieve .NET employees with vacations", ex);
        }
    }

    public async Task<List<EmployeeVacationSummaryDto>> GetEmployeesWithUsedVacationDaysAsync()
    {
        try
        {
            var currentYear = DateTime.Now.Year;
            _logger.LogInformation("Retrieving employees with used vacation days for year {Year}", currentYear);

            var result = await _repository.GetEmployeesWithUsedVacationDaysAsync(currentYear);

            _logger.LogInformation("Retrieved vacation summaries for {Count} employees", result.Count);
            return result;
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving employee vacation summaries");
            throw new BusinessLogicException("Failed to retrieve employee vacation summaries", ex);
        }
    }

    public async Task<List<TeamDto>> GetTeamsWithoutVacationsIn2019Async()
    {
        try
        {
            _logger.LogInformation("Retrieving teams without vacations in 2019");

            var teams = await _repository.GetTeamsWithoutVacationsInYearAsync(2019);

            var result = teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            _logger.LogInformation("Found {Count} teams without vacations in 2019", result.Count);
            return result;
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving teams without vacations in 2019");
            throw new BusinessLogicException("Failed to retrieve teams without vacations", ex);
        }
    }
}