using HR.Domain.Leaves;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly HRDbContext _dbContext;

        public LeaveRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeaveRequest?> GetRequestByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.ReplacementEmployee)
                .FirstOrDefaultAsync(lr => lr.Id == id, ct);
        }

        public async Task<IReadOnlyList<LeaveRequest>> GetRequestsByCategoryAsync(LeaveCategory category, CancellationToken ct = default)
        {
            return await _dbContext.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.ReplacementEmployee)
                .Where(lr => lr.LeaveCategory == category)
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<LeaveRequest>> GetRequestsByEmployeeAsync(Guid employeeId, int year, CancellationToken ct = default)
        {
            return await _dbContext.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.EmployeeId == employeeId && lr.StartDate.Year == year)
                .OrderByDescending(lr => lr.CreatedAt)
                .ToListAsync(ct);
        }

        public void AddRequest(LeaveRequest request)
        {
            _dbContext.LeaveRequests.Add(request);
        }

        public void UpdateRequest(LeaveRequest request)
        {
            _dbContext.LeaveRequests.Update(request);
        }

        public async Task<LeaveBalance?> GetBalanceAsync(Guid employeeId, int year, CancellationToken ct = default)
        {
            return await _dbContext.LeaveBalances
                .Include(lb => lb.Employee)
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.Year == year, ct);
        }

        public void AddBalance(LeaveBalance balance)
        {
            _dbContext.LeaveBalances.Add(balance);
        }

        public void UpdateBalance(LeaveBalance balance)
        {
            _dbContext.LeaveBalances.Update(balance);
        }
    }
}
