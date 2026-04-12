using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
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
                .Include(e => e.EmployeeFamilies)
                .Include(e => e.EmployeeQualifications)
                .Include(e => e.EmployeeDecisions)
                // .Include(e => e.LeadershipHistory)
                .FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public async Task<Employee> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeFamilies)
                .Include(e => e.EmployeeQualifications)
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
        public async Task<string> GetNextCodeAsync(CancellationToken cancellationToken = default)
        {
            // Fetch the maximum code currently in the database.
            // Note: Since Code is a string, "00002" sorts correctly after "00001".
            var maxCodeStr = await _dbContext.Set<Employee>()
                .OrderByDescending(e => e.Code)
                .Select(e => e.Code)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(maxCodeStr))
            {
                // Base case: No employees exist yet.
                return "00001"; // Or "EMP-00001" depending on your pattern
            }

            // ---------------------------------------------------------
            // PATTERN A: Padded Numeric Codes (e.g., "00001", "00002")
            // ---------------------------------------------------------
            if (int.TryParse(maxCodeStr, out int currentMaxCode))
            {
                // Increment and format back to a 5-digit string with leading zeros
                return (currentMaxCode + 1).ToString("D5");
            }

            // ---------------------------------------------------------
            // PATTERN B: Prefixed Codes (e.g., "EMP-00001", "EMP-00002")
            // (Uncomment this block and delete Pattern A if you use prefixes)
            // ---------------------------------------------------------
            /*
            string prefix = "EMP-";
            if (maxCodeStr.StartsWith(prefix))
            {
                string numericPart = maxCodeStr.Substring(prefix.Length);
                if (int.TryParse(numericPart, out int maxCodeNum))
                {
                    return $"{prefix}{(maxCodeNum + 1).ToString("D5")}";
                }
            }
            */

            // If the code format in the DB doesn't match our expectations
            throw new InvalidOperationException("Unrecognized employee code format in the database.");
        }
    }
}
