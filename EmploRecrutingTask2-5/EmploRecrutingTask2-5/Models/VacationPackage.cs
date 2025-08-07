namespace EmploRecrutingTask2_5.Models;

public class VacationPackage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GrantedDays { get; set; }
    public int Year { get; set; }
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
