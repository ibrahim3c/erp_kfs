using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Repositories
{
    public class JobGradeRepository : BaseRepository<JobGrade>, IJobGradeRepository
    {
        public JobGradeRepository(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
    }
}