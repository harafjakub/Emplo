using EmploRecrutingTask2_5.Models;

namespace EmploRecrutingTask2_5.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeWithVacationPackageAsync(int employeeId);
        Task<List<Vacation>> GetEmployeeVacationsAsync(int employeeId);
    }
}
