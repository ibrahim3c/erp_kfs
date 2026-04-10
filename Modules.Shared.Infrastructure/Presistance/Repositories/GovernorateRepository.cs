using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Infrastructure.Presistance.Database;


namespace Modules.Shared.Infrastructure.Presistance.Repositories
{
    public class GovernorateRepository : IGovernorateRepository
    {
        private readonly SharedDbContext dbContext;

        public GovernorateRepository(SharedDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(Governorate candidate)
        {
            dbContext.Governorates.Add(candidate);
        }

        public void Delete(Governorate candidate)
        {
            dbContext.Governorates.Remove(candidate);
        }

        public async Task<List<Governorate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Governorates.ToListAsync(cancellationToken);
        }

        public async Task<Governorate> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Governorates.FindAsync(id, cancellationToken);
        }

        public async Task<Governorate> GetByIdWithCityCentersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Governorates.Include(g => g.CityCenters).FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public void Update(Governorate candidate)
        {
            dbContext.Governorates.Update(candidate);
        }
    }
}
