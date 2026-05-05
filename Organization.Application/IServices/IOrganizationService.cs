using Organization.Application.Dtos.OrgUnit;
using Organization.Application.Dtos.OrgUnitType;
using Organization.Application.Dtos.LeadershipPosition;
using Organization.Application.Dtos.LeadershipPositionHistory;
using Organization.Application.Dtos.QualitativeGroup;
using Organization.Application.Dtos.FunctionalGroup;
using Organization.Application.Dtos.JobTitle;
using Organization.Application.Dtos.JobGrade;
using Modules.Shared.Domain;

namespace Organization.Application.IServices
{
    public interface IOrganizationService
    {
        // OrgUnitType
        Task<Result<IEnumerable<OrgUnitTypeDto>>> GetAllOrgUnitTypesAsync();
        Task<Result<OrgUnitTypeDto>> GetOrgUnitTypeByIdAsync(Guid id);
        Task<Result<Guid>> CreateOrgUnitTypeAsync(CreateOrgUnitTypeDto dto);
        Task<Result<bool>> UpdateOrgUnitTypeAsync(UpdateOrgUnitTypeDto dto);
        Task<Result<bool>> DeleteOrgUnitTypeAsync(Guid id);

        // OrgUnit
        Task<Result<IEnumerable<OrgUnitDto>>> GetAllOrgUnitsAsync();
        Task<Result<OrgUnitDto>> GetOrgUnitByIdAsync(Guid id);
        Task<Result<Guid>> CreateOrgUnitAsync(CreateOrgUnitDto dto);
        Task<Result<bool>> UpdateOrgUnitAsync(UpdateOrgUnitDto dto);
        Task<Result<bool>> DeleteOrgUnitAsync(Guid id);

        // LeadershipPosition
        Task<Result<IEnumerable<LeadershipPositionDto>>> GetAllLeadershipPositionsAsync();
        Task<Result<LeadershipPositionDto>> GetLeadershipPositionByIdAsync(Guid id);
        Task<Result<Guid>> CreateLeadershipPositionAsync(CreateLeadershipPositionDto dto);
        Task<Result<LeadershipPositionHistoryDto>> GetLeadershipPositionHistoriesByEmployeeIdAsync(Guid employeeId);
        Task<Result<bool>> UpdateLeadershipPositionAsync(UpdateLeadershipPositionDto dto);
        Task<Result<bool>> DeleteLeadershipPositionAsync(Guid id);

        // LeadershipPositionHistory
        Task<Result<IEnumerable<LeadershipPositionHistoryDto>>> GetAllLeadershipPositionHistoriesAsync();
        Task<Result<LeadershipPositionHistoryDto>> GetLeadershipPositionHistoryByIdAsync(Guid id);
        Task<Result<Guid>> CreateLeadershipPositionHistoryAsync(CreateLeadershipPositionHistoryDto dto);
        Task<Result<bool>> UpdateLeadershipPositionHistoryAsync(UpdateLeadershipPositionHistoryDto dto);
        Task<Result<bool>> DeleteLeadershipPositionHistoryAsync(Guid id);

        // QualitativeGroup
        Task<Result<IEnumerable<QualitativeGroupDto>>> GetAllQualitativeGroupsAsync();
        Task<Result<QualitativeGroupDto>> GetQualitativeGroupByIdAsync(Guid id);
        Task<Result<Guid>> CreateQualitativeGroupAsync(CreateQualitativeGroupDto dto);
        Task<Result<bool>> UpdateQualitativeGroupAsync(UpdateQualitativeGroupDto dto);
        Task<Result<bool>> DeleteQualitativeGroupAsync(Guid id);

        // FunctionalGroup
        Task<Result<IEnumerable<FunctionalGroupDto>>> GetAllFunctionalGroupsAsync();
        Task<Result<FunctionalGroupDto>> GetFunctionalGroupByIdAsync(Guid id);
        Task<Result<Guid>> CreateFunctionalGroupAsync(CreateFunctionalGroupDto dto);
        Task<Result<bool>> UpdateFunctionalGroupAsync(UpdateFunctionalGroupDto dto);
        Task<Result<bool>> DeleteFunctionalGroupAsync(Guid id);

        // JobTitle
        Task<Result<IEnumerable<JobTitleDto>>> GetAllJobTitlesAsync();
        Task<Result<JobTitleDto>> GetJobTitleByIdAsync(Guid id);
        Task<Result<Guid>> CreateJobTitleAsync(CreateJobTitleDto dto);
        Task<Result<bool>> UpdateJobTitleAsync(UpdateJobTitleDto dto);
        Task<Result<bool>> DeleteJobTitleAsync(Guid id);

        // JobGrade
        Task<Result<IEnumerable<JobGradeDto>>> GetAllJobGradesAsync();
        Task<Result<JobGradeDto>> GetJobGradeByIdAsync(Guid id);
        Task<Result<Guid>> CreateJobGradeAsync(CreateJobGradeDto dto);
        Task<Result<bool>> UpdateJobGradeAsync(UpdateJobGradeDto dto);
        Task<Result<bool>> DeleteJobGradeAsync(Guid id);
    }
}