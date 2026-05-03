using HR.Domain.Penalties;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;


namespace HR.Infrastructure.Persistance.Repositories
{
    public class PenaltyRepository : IPenaltyRepository
    {
        private readonly HRDbContext dbContext;

        public PenaltyRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(PenaltyRecord penalty)
        {
           dbContext.PenaltyRecords.Add(penalty);
        }

        public void Delete(PenaltyRecord penalty)
        {
            dbContext.PenaltyRecords.Remove(penalty);
        }

        public async Task<List<PenaltyRecord>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.PenaltyRecords.Include(p => p.Employee).AsNoTracking()
                                                 .OrderByDescending(p => p.ViolationDate).ToListAsync(cancellationToken);
        }

        public async Task<List<PenaltyRecord>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await dbContext.PenaltyRecords.Where(x => x.EmployeeId == employeeId)
                .AsNoTracking().ToListAsync(cancellationToken);

        }

        public async Task<PenaltyRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           return await dbContext.PenaltyRecords.Include(x =>x.Employee).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Update(PenaltyRecord penalty)
        {
            dbContext.PenaltyRecords.Update(penalty);
        }
    }
}
