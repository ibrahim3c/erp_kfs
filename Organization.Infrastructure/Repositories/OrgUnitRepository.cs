using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class OrgUnitRepository : BaseRepository<OrgUnit>, IOrgUnitRepository
    {
        public OrgUnitRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}