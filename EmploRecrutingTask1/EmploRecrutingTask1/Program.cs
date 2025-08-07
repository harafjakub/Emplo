using EmploRecrutingTask1.Models;
using EmploRecrutingTask1.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IEmployeeHierarchyService, EmployeeHierarchyService>()
    .BuildServiceProvider();

var hierarchyService = serviceProvider.GetRequiredService<IEmployeeHierarchyService>();

var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Jan Kowalski", SuperiorId = null },
    new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
    new Employee { Id = 3, Name = "Anna Mariacka", SuperiorId = 1 },
    new Employee { Id = 4, Name = "Andrzej Abacki", SuperiorId = 2 }
};
            
try
{
    hierarchyService.FillEmployeesStructure(employees);
    
    var row1 = hierarchyService.GetSuperiorRowOfEmployee(2, 1); // should return 1
    var row2 = hierarchyService.GetSuperiorRowOfEmployee(4, 3); // should return null
    var row3 = hierarchyService.GetSuperiorRowOfEmployee(4, 1); // should return 2

    Console.WriteLine($"GetSuperiorRowOfEmployee(2, 1): {row1?.ToString() ?? "null"}");
    Console.WriteLine($"GetSuperiorRowOfEmployee(4, 3): {row2?.ToString() ?? "null"}");
    Console.WriteLine($"GetSuperiorRowOfEmployee(4, 1): {row3?.ToString() ?? "null"}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
