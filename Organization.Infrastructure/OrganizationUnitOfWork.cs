using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Organization.Infrastructure.Repositories;

namespace Organization.Infrastructure
{
    internal class OrganizationUnitOfWork : IOrganizationUnitOfWork
    {
        private readonly OrganizationDbContext _dbContext;

        public OrganizationUnitOfWork(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
            OrgUnitTypeRepository = new OrgUnitTypeRepository(_dbContext);
            OrgUnitRepository = new OrgUnitRepository(_dbContext);
            LeadershipPositionRepository = new LeadershipPositionRepository(_dbContext);
            LeadershipPositionHistoryRepository = new LeadershipPositionHistoryRepository(_dbContext);
            QualitativeGroupRepository = new QualitativeGroupRepository(_dbContext);
            FunctionalGroupRepository = new FunctionalGroupRepository(_dbContext);
            JobTitleRepository = new JobTitleRepository(_dbContext);
            JobGradeRepository = new JobGradeRepository(_dbContext);
        }

        public IOrgUnitTypeRepository OrgUnitTypeRepository { get; private set; }
        public IOrgUnitRepository OrgUnitRepository { get; private set; }
        public ILeadershipPositionRepository LeadershipPositionRepository { get; private set; }
        public ILeadershipPositionHistoryRepository LeadershipPositionHistoryRepository { get; private set; }
        public IQualitativeGroupRepository QualitativeGroupRepository { get; private set; }
        public IFunctionalGroupRepository FunctionalGroupRepository { get; private set; }
        public IJobTitleRepository JobTitleRepository { get; private set; }
        public IJobGradeRepository JobGradeRepository { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}