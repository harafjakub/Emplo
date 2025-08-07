using EmploRecrutingTask2_5.DTOs;
using EmploRecrutingTask2_5.Exceptions;
using EmploRecrutingTask2_5.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploRecrutingTask2_5.Repositories;

public class VacationRepository : IVacationRepository
{
    private readonly VacationDbContext _context;

    public VacationRepository(VacationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Employee>> GetDotNetEmployeesWithVacationsInYearAsync(int year)
    {
        try
        {
            return await _context.Employees
                .Include(e => e.Team)
                .Where(e => e.Team.Name == ".NET")
                .Where(e => _context.Vacations
                    .Any(v => v.EmployeeId == e.Id && 
                        ((v.DateSince.Year == year) || (v.DateUntil.Year == year))))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Error retrieving .NET employees with vacations in {year}", ex);
        }
    }

    public async Task<List<EmployeeVacationSummaryDto>> GetEmployeesWithUsedVacationDaysAsync(int year)
    {
        try
        {
            var today = DateTime.Today;

            var employeesWithVacations = await _context.Employees
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    Vacations = _context.Vacations
                        .Where(v => v.EmployeeId == e.Id && 
                                    v.DateUntil < today &&
                                    v.DateSince.Year <= year && v.DateUntil.Year >= year)
                        .Select(v => new { v.DateSince, v.DateUntil })
                        .ToList()
                })
                .ToListAsync();

            return employeesWithVacations.Select(e => new EmployeeVacationSummaryDto
            {
                Id = e.Id,
                Name = e.Name,
                UsedVacationDays = e.Vacations.Sum(v => {
                    var startDate = v.DateSince.Year < year 
                        ? new DateTime(year, 1, 1) 
                        : v.DateSince;

                    var endDate = v.DateUntil.Year > year 
                        ? new DateTime(year, 12, 31) 
                        : v.DateUntil;
                    
                    if (endDate < startDate)
                        return 0;

                    return (endDate - startDate).Days + 1;
                })
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Error retrieving employee vacation summaries for {year}", ex);
        }
    }

    public async Task<List<Team>> GetTeamsWithoutVacationsInYearAsync(int year)
    {
        try
        {
            var teamsWithVacations = await _context.Vacations
                .Where(v => (v.DateSince.Year == year) || 
                            (v.DateUntil.Year == year))
                .Select(v => v.Employee.TeamId)
                .Distinct()
                .ToListAsync();

            return await _context.Teams
                .Where(t => !teamsWithVacations.Contains(t.Id))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Error retrieving teams without vacations in {year}", ex);
        }
    }
}