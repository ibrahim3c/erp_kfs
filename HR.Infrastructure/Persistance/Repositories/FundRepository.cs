using HR.Domain.Funds;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class FundRepository : IFundRepository
    {
        private readonly HRDbContext _dbContext;

        public FundRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FundSubscription?> GetSubscriptionByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.FundSubscriptions
                .Include(fs => fs.Employee)
                .FirstOrDefaultAsync(fs => fs.Id == id, ct);
        }

        public async Task<IReadOnlyList<FundSubscription>> GetAllSubscriptionsAsync(CancellationToken ct = default)
        {
            return await _dbContext.FundSubscriptions
                .Include(fs => fs.Employee)
                .OrderByDescending(fs => fs.SubscriptionDate)
                .ToListAsync(ct);
        }

        public async Task<FundSubscription?> GetActiveSubscriptionByEmployeeAsync(Guid employeeId, FundType fundType, CancellationToken ct = default)
        {
            return await _dbContext.FundSubscriptions
                .FirstOrDefaultAsync(fs =>
                    fs.EmployeeId == employeeId &&
                    fs.FundType == fundType &&
                    fs.Status == FundSubscriptionStatus.Active, ct);
        }

        public void AddSubscription(FundSubscription subscription)
        {
            _dbContext.FundSubscriptions.Add(subscription);
        }

        public void UpdateSubscription(FundSubscription subscription)
        {
            _dbContext.FundSubscriptions.Update(subscription);
        }

        public async Task<FundClaim?> GetClaimByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.FundClaims
                .Include(fc => fc.Employee)
                .FirstOrDefaultAsync(fc => fc.Id == id, ct);
        }

        public async Task<IReadOnlyList<FundClaim>> GetAllClaimsAsync(CancellationToken ct = default)
        {
            return await _dbContext.FundClaims
                .Include(fc => fc.Employee)
                .OrderByDescending(fc => fc.CreatedAt)
                .ToListAsync(ct);
        }

        public void AddClaim(FundClaim claim)
        {
            _dbContext.FundClaims.Add(claim);
        }

        public void UpdateClaim(FundClaim claim)
        {
            _dbContext.FundClaims.Update(claim);
        }
    }
}
