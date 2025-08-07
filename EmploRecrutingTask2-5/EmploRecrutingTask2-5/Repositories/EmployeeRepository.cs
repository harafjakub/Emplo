using EmploRecrutingTask2_5.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploRecrutingTask2_5.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly VacationDbContext _dbContext;

        public EmployeeRepository(VacationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Employee?> GetEmployeeWithVacationPackageAsync(int employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.VacationPackage)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<List<Vacation>> GetEmployeeVacationsAsync(int employeeId)
        {
            return await _dbContext.Vacations
                .Where(v => v.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
