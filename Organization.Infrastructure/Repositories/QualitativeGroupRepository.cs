using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class QualitativeGroupRepository : BaseRepository<QualitativeGroup>, IQualitativeGroupRepository
    {
        public QualitativeGroupRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}