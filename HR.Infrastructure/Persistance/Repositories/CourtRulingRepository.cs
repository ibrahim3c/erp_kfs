using HR.Domain.Legal;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class CourtRulingRepository : ICourtRulingRepository
    {
        private readonly HRDbContext _dbContext;

        public CourtRulingRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CourtRuling?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.CourtRulings
                .FirstOrDefaultAsync(cr => cr.Id == id, ct);
        }

        public async Task<IReadOnlyList<CourtRuling>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.CourtRulings
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync(ct);
        }

        public void Add(CourtRuling ruling)
        {
            _dbContext.CourtRulings.Add(ruling);
        }

        public void Update(CourtRuling ruling)
        {
            _dbContext.CourtRulings.Update(ruling);
        }
    }
}
