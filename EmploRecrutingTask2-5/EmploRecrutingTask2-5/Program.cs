using EmploRecrutingTask2_5;
using EmploRecrutingTask2_5.Configuration;
using EmploRecrutingTask2_5.Data;
using EmploRecrutingTask2_5.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddVacationServices("Data Source=vacation.db");
builder.Services.AddScoped<DataSeeder>();

var host = builder.Build();

try
{
    using var scope = host.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<VacationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    await context.Database.EnsureCreatedAsync();
    await dataSeeder.SeedAsync();

    await RunTask2(scope, logger);
    await RunTask3(scope, context, logger);
    await RunTask4(scope, context, logger);
}
catch (Exception ex)
{
    Console.WriteLine($"Critical application error: {ex.Message}");
    return 1;
}

return 0;

async Task RunTask2(IServiceScope scope, ILogger<Program> logger)
{
    Console.WriteLine("\n╔════════════════════════════════════════╗");
    Console.WriteLine("║              ZADANIE 2                 ║");
    Console.WriteLine("╚════════════════════════════════════════╝");

    var vacationService = scope.ServiceProvider.GetRequiredService<IVacationService>();

    Console.WriteLine("\n=== Pracownicy z zespołu .NET z urlopami w 2019 ===");
    try
    {
        var dotNetEmployees = await vacationService.GetDotNetEmployeesWithVacationsIn2019Async();
        foreach (var employee in dotNetEmployees)
        {
            Console.WriteLine($"{employee.Id}: {employee.Name} (Team: {employee.TeamName})");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving .NET employees");
        Console.WriteLine("Błąd podczas pobierania pracowników .NET");
    }

    Console.WriteLine("\n=== Pracownicy z liczbą zużytych dni urlopowych ===");
    try
    {
        var employeesWithDays = await vacationService.GetEmployeesWithUsedVacationDaysAsync();
        foreach (var item in employeesWithDays)
        {
            Console.WriteLine($"{item.Id}: {item.Name} - {item.UsedVacationDays} dni");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving employee vacation summaries");
        Console.WriteLine("Błąd podczas pobierania podsumowań urlopów");
    }

    Console.WriteLine("\n=== Zespoły bez urlopów w 2019 ===");
    try
    {
        var teamsWithoutVacations = await vacationService.GetTeamsWithoutVacationsIn2019Async();
        foreach (var team in teamsWithoutVacations)
        {
            Console.WriteLine($"{team.Id}: {team.Name}");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving teams without vacations");
        Console.WriteLine("Błąd podczas pobierania zespołów bez urlopów");
    }
}

async Task RunTask3(IServiceScope scope, VacationDbContext context, ILogger<Program> logger)
{
    Console.WriteLine("\n╔════════════════════════════════════════╗");
    Console.WriteLine("║              ZADANIE 3                 ║");
    Console.WriteLine("╚════════════════════════════════════════╝");

    Console.WriteLine("\n=== Pozostałe dni urlopowe dla pracowników ===");
    try
    {
        var vacationCalculatorService = scope.ServiceProvider.GetRequiredService<IVacationCalculatorService>();

        var employees = await context.Employees.ToListAsync();
        foreach (var employee in employees)
        {
            var remainingDays = await vacationCalculatorService.CountFreeDaysForEmployeeAsync(employee.Id);
            Console.WriteLine($"{employee.Id}: {employee.Name} - pozostało {remainingDays} dni urlopowych");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error calculating remaining vacation days");
        Console.WriteLine("Błąd podczas obliczania pozostałych dni urlopowych");
    }
}

async Task RunTask4(IServiceScope scope, VacationDbContext context, ILogger<Program> logger)
{
    Console.WriteLine("\n╔════════════════════════════════════════╗");
    Console.WriteLine("║              ZADANIE 4                 ║");
    Console.WriteLine("╚════════════════════════════════════════╝");

    Console.WriteLine("\n=== Możliwość zgłoszenia wniosku urlopowego ===");
    try
    {
        var vacationCalculatorService = scope.ServiceProvider.GetRequiredService<IVacationCalculatorService>();

        var employees = await context.Employees.ToListAsync();
        foreach (var employee in employees)
        {
            var canRequest = await vacationCalculatorService.IfEmployeeCanRequestVacationAsync(employee.Id);
            Console.WriteLine($"{employee.Id}: {employee.Name} - możliwość złożenia wniosku: {(canRequest ? "TAK" : "NIE")}");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error checking vacation request possibility");
        Console.WriteLine("Błąd podczas sprawdzania możliwości złożenia wniosku urlopowego");
    }
}