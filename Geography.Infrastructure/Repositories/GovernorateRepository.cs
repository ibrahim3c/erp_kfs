using Geography.Domain;
using Geography.Domain.IRepositories;
using Geography.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Infrastructure.Repositories
{
    public class GovernorateRepository : BaseRepository<Governorate>, IGovernorateRepository
    {
        public GovernorateRepository(GeographyDbContext dbContext) : base(dbContext)
        {
        }
    }

}
