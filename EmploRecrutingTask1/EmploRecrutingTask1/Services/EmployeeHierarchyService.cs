using EmploRecrutingTask1.Models;

namespace EmploRecrutingTask1.Services;

/// <summary>
/// Service for managing employee hierarchy relationships
/// </summary>
public class EmployeeHierarchyService : IEmployeeHierarchyService
{
    private List<EmployeeStructure> _employeeStructures;
    private Dictionary<int, Employee> _employeesLookup;

    public EmployeeHierarchyService()
    {
        _employeeStructures = new List<EmployeeStructure>();
        _employeesLookup = new Dictionary<int, Employee>();
    }

    /// <summary>
    /// Fills the employee structure with all hierarchical relationships
    /// </summary>
    /// <param name="employees">List of all employees</param>
    /// <returns>List of employee structures representing all superior-subordinate relationships</returns>
    public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
    {
        if (employees == null || !employees.Any())
        {
            throw new ArgumentException("Employee list cannot be null or empty", nameof(employees));
        }

        _employeeStructures.Clear();
        _employeesLookup.Clear();

        _employeesLookup = employees.ToDictionary(emp => emp.Id, emp => emp);

        ValidateEmployeeData(employees);

        foreach (var employee in employees)
        {
            BuildHierarchyForEmployee(employee.Id);
        }

        return new List<EmployeeStructure>(_employeeStructures);
    }

    /// <summary>
    /// Returns the superior level between two employees
    /// </summary>
    /// <param name="employeeId">ID of the employee</param>
    /// <param name="superiorId">ID of the potential superior</param>
    /// <returns>Level of superior relationship, or null if no relationship exists</returns>
    public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
    {
        if (employeeId == superiorId) return null;

        var relationship = _employeeStructures
            .FirstOrDefault(es => es.EmployeeId == employeeId && es.SuperiorId == superiorId);

        return relationship?.Level;
    }

    /// <summary>
    /// Builds hierarchy relationships for a specific employee
    /// </summary>
    /// <param name="employeeId">ID of the employee</param>
    private void BuildHierarchyForEmployee(int employeeId)
    {
        var visitedEmployees = new HashSet<int>();
        var currentEmployeeId = employeeId;
        var level = 1;

        while (_employeesLookup.TryGetValue(currentEmployeeId, out var currentEmployee) 
               && currentEmployee.SuperiorId.HasValue)
        {
            if (visitedEmployees.Contains(currentEmployee.SuperiorId.Value))
            {
                throw new InvalidOperationException(
                    $"Circular reference detected in employee hierarchy for employee {employeeId}");
            }

            var superiorId = currentEmployee.SuperiorId.Value;

            if (!_employeesLookup.ContainsKey(superiorId))
            {
                throw new InvalidOperationException(
                    $"Superior with ID {superiorId} does not exist for employee {currentEmployeeId}");
            }

            if (!_employeeStructures.Any(es => es.EmployeeId == employeeId && es.SuperiorId == superiorId))
            {
                _employeeStructures.Add(new EmployeeStructure(employeeId, superiorId, level));
            }

            visitedEmployees.Add(currentEmployeeId);
            currentEmployeeId = superiorId;
            level++;
        }
    }

    /// <summary>
    /// Validates employee data for consistency
    /// </summary>
    /// <param name="employees">List of employees to validate</param>
    private void ValidateEmployeeData(List<Employee> employees)
    {
        var employeeIds = employees.Select(e => e.Id).ToList();
            
        if (employeeIds.Count != employeeIds.Distinct().Count())
        {
            throw new ArgumentException("Duplicate employee IDs found in the employee list");
        }

        foreach (var employee in employees.Where(e => e.SuperiorId.HasValue))
        {
            if (!employeeIds.Contains(employee.SuperiorId!.Value))
            {
                throw new ArgumentException(
                    $"Employee {employee.Id} references non-existent superior {employee.SuperiorId}");
            }

            if (employee.Id == employee.SuperiorId.Value)
            {
                throw new ArgumentException(
                    $"Employee {employee.Id} cannot be their own superior");
            }
        }
    }
}