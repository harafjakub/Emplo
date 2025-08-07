using EmploRecrutingTask2_5.Repositories;
using EmploRecrutingTask2_5.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmploRecrutingTask2_5.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVacationServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<VacationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IVacationRepository, VacationRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IVacationService, VacationService>();
        services.AddScoped<IVacationCalculatorService, VacationCalculatorService>();

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Warning);
        });

        return services;
    }
}
