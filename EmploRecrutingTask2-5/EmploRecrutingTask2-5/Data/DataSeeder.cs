using EmploRecrutingTask2_5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmploRecrutingTask2_5.Data;

public class DataSeeder
{
    private readonly VacationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(VacationDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedAsync()
    {
        if (await _context.Teams.AnyAsync())
        {
            _logger.LogInformation("Database already contains data, skipping seed");
            return;
        }

        _logger.LogInformation("Seeding database with sample data");

        // Zespoły
        var dotNetTeam = new Team { Name = ".NET" };
        var javaTeam = new Team { Name = "Java" };
        _context.Teams.AddRange(dotNetTeam, javaTeam);
        await _context.SaveChangesAsync();

        // Pakiety urlopowe
        var package2019 = new VacationPackage { Name = "Standard 2019", GrantedDays = 26, Year = 2019 };
        var package2025 = new VacationPackage { Name = "Standard 2025", GrantedDays = 26, Year = 2025 };
        _context.VacationPackages.AddRange(package2019, package2025);
        await _context.SaveChangesAsync();

        // Pracownicy
        var emp1 = new Employee { Name = "Jan Kowalski", TeamId = dotNetTeam.Id, PositionId = 1, VacationPackageId = package2025.Id };
        var emp2 = new Employee { Name = "Anna Nowak", TeamId = dotNetTeam.Id, PositionId = 2, VacationPackageId = package2025.Id };
        var emp3 = new Employee { Name = "Piotr Wiśniewski", TeamId = javaTeam.Id, PositionId = 1, VacationPackageId = package2025.Id };
        _context.Employees.AddRange(emp1, emp2, emp3);
        await _context.SaveChangesAsync();

        // Urlopy
        var vacations = new List<Vacation>
        {
            new() { 
                DateSince = new DateTime(2019, 7, 1),
                DateUntil = new DateTime(2019, 7, 14),
                NumberOfHours = 112,
                IsPartialVacation = false,
                EmployeeId = emp1.Id
            },
            new() { 
                DateSince = new DateTime(2018, 12, 27),
                DateUntil = new DateTime(2019, 1, 5),
                NumberOfHours = 64,
                IsPartialVacation = false,
                EmployeeId = emp2.Id
            },
            new() {
                DateSince = new DateTime(2024, 12, 20),
                DateUntil = new DateTime(2025, 1, 3),
                NumberOfHours = 56,
                IsPartialVacation = false,
                EmployeeId = emp1.Id
            },
            new() {
                DateSince = new DateTime(2025, 6, 1),
                DateUntil = new DateTime(2025, 6, 23),
                NumberOfHours = 56,
                IsPartialVacation = false,
                EmployeeId = emp1.Id
            },
            new() {
                DateSince = new DateTime(2025, 8, 1),
                DateUntil = new DateTime(2025, 8, 5),
                NumberOfHours = 40,
                IsPartialVacation = false,
                EmployeeId = emp2.Id
            }
        };

        _context.Vacations.AddRange(vacations);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Database seeding completed successfully");
    }
}
