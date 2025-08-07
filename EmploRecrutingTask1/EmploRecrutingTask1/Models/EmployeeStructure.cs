namespace EmploRecrutingTask1.Models;

/// <summary>
/// Represents a hierarchical relationship between employees
/// </summary>
public class EmployeeStructure
{
    public int EmployeeId { get; set; }
    public int SuperiorId { get; set; }
    public int Level { get; set; }
        
    public EmployeeStructure(int employeeId, int superiorId, int level)
    {
        EmployeeId = employeeId;
        SuperiorId = superiorId;
        Level = level;
    }
}
