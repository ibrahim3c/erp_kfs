using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class FunctionalGroupRepository : BaseRepository<FunctionalGroup>, IFunctionalGroupRepository
    {
        public FunctionalGroupRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}