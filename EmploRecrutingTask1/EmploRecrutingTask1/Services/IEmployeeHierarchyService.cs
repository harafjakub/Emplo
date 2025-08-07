using EmploRecrutingTask1.Models;

namespace EmploRecrutingTask1.Services;

/// <summary>
/// Interface for managing employee hierarchy relationships
/// </summary>
public interface IEmployeeHierarchyService
{
    /// <summary>
    /// Fills the employee structure with all hierarchical relationships
    /// </summary>
    /// <param name="employees">List of all employees</param>
    /// <returns>List of employee structures representing all superior-subordinate relationships</returns>
    List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees);

    /// <summary>
    /// Returns the superior level between two employees
    /// </summary>
    /// <param name="employeeId">ID of the employee</param>
    /// <param name="superiorId">ID of the potential superior</param>
    /// <returns>Level of superior relationship, or null if no relationship exists</returns>
    int? GetSuperiorRowOfEmployee(int employeeId, int superiorId);
}
