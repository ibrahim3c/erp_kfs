using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class LeadershipPositionRepository : BaseRepository<LeadershipPosition>, ILeadershipPositionRepository
    {
        public LeadershipPositionRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}