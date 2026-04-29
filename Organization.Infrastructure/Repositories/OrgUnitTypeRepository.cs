using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class OrgUnitTypeRepository : BaseRepository<OrgUnitType>, IOrgUnitTypeRepository
    {
        public OrgUnitTypeRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}