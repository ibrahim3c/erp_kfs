using Geography.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Domain.Repositories
{
    public class VillageRepository : BaseRepository<Village>, IVillageRepository
    {
        public VillageRepository(GeographyDbContext dbContext) : base(dbContext)
        {
        }
    }
}