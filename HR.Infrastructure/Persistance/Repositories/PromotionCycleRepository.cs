using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Interfaces;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;


namespace HR.Infrastructure.Persistance.Repositories
{
    internal class PromotionCycleRepository : IPromotionCycleRepository
    {
        private readonly HRDbContext _db;
        public PromotionCycleRepository(HRDbContext db) => _db = db;

 
        public async Task<PromotionCycle?> GetByIdAsync(Guid id, CancellationToken ct)
            => await _db.PromotionCycles
                .Include(c => c.Results)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<Guid> SaveCycleAsync(PromotionCycle cycle, CancellationToken ct)
        {
            _db.PromotionCycles.Add(cycle);
            await _db.SaveChangesAsync(ct);
            return cycle.Id;

        }
        public async Task AddResultsAsync(IEnumerable<EligibilityResult> results, CancellationToken ct)
        {
            await _db.EligibilityResults.AddRangeAsync(results, ct);
        }

        public void Update(PromotionCycle cycle)
        {
            _db.PromotionCycles.Update(cycle);
        }
    }
}
