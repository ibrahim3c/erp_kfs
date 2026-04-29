using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class LeadershipPositionHistoryRepository : BaseRepository<LeadershipPositionHistory>, ILeadershipPositionHistoryRepository
    {
        public LeadershipPositionHistoryRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}