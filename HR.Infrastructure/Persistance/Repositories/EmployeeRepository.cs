using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
namespace HR.Infrastructure.Persistance.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRDbContext _dbContext;

        public EmployeeRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // ملاحظة: لو كنت تستخدم Guid بدلاً من int لـ Id، قم بتعديل int إلى Guid هنا وفي الـ Interface
            return await _dbContext.Employees
                .Include(e => e.Families)
                .Include(e => e.Qualifications)
                .Include(e => e.Decisions)
                // .Include(e => e.LeadershipHistory)
                .FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public async Task<Employee> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.Families)
                .Include(e => e.Qualifications)
                .FirstOrDefaultAsync(e => e.NationalId == nationalId, cancellationToken);
        }

        public void Add(Employee employee)
        {
            _dbContext.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            _dbContext.Employees.Update(employee);
        }

        public void Delete(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
        }
    }
}
