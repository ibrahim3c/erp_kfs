using CollegeControlSystem.Domain.Abstractions;
using Organization.Domain.IRepositories;

namespace Organization.Domain
{
    public interface IOrganizationUnitOfWork : IUnitOfWork
    {
        IOrgUnitTypeRepository OrgUnitTypeRepository { get; }
        IOrgUnitRepository OrgUnitRepository { get; }
        ILeadershipPositionRepository LeadershipPositionRepository { get; }
        ILeadershipPositionHistoryRepository LeadershipPositionHistoryRepository { get; }
        IQualitativeGroupRepository QualitativeGroupRepository { get; }
        IFunctionalGroupRepository FunctionalGroupRepository { get; }
        IJobTitleRepository JobTitleRepository { get; }
        IJobGradeRepository JobGradeRepository { get; }
    }
}