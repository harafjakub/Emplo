namespace EmploRecrutingTask2_5.DTOs;

public class EmployeeVacationSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UsedVacationDays { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Name} - {UsedVacationDays} dni";
    }
}
