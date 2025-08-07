namespace EmploRecrutingTask2_5.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public int PositionId { get; set; }
    public int VacationPackageId { get; set; }
    
    public Team Team { get; set; } = null!;
    public VacationPackage VacationPackage { get; set; } = null!;
    public ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
}
