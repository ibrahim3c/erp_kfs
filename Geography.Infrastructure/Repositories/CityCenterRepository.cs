using Geography.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Domain.Repositories
{
    public class CityCenterRepository : BaseRepository<CityCenter>, ICityCenterRepository
    {
        public CityCenterRepository(GeographyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
