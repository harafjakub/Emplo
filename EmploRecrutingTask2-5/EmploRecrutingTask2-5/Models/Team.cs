namespace EmploRecrutingTask2_5.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
