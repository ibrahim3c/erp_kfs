using Organization.Application.Dtos.LeadershipPosition;
using Organization.Application.Dtos.LeadershipPositionHistory;
using Organization.Application.Dtos.OrgUnit;
using Organization.Application.Dtos.OrgUnitType;
using Organization.Application.Dtos.QualitativeGroup;
using Organization.Application.Dtos.FunctionalGroup;
using Organization.Application.Dtos.JobTitle;
using Organization.Application.Dtos.JobGrade;
using Organization.Application.IServices;
using Organization.Domain;
using Organization.Domain.IRepositories;
using Modules.Shared.Domain;

namespace Organization.Infrastructure.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationUnitOfWork _unitOfWork;

        public OrganizationService(IOrganizationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // OrgUnitType
        public async Task<Result<IEnumerable<OrgUnitTypeDto>>> GetAllOrgUnitTypesAsync()
        {
            var orgUnitTypes = await _unitOfWork.OrgUnitTypeRepository.GetAllAsync();
            var dtoList = orgUnitTypes.Select(x => new OrgUnitTypeDto(x.Id, x.Code, x.Name, x.LevelOrder, x.CanHaveChild));
            return Result<IEnumerable<OrgUnitTypeDto>>.Success(dtoList);
        }

        public async Task<Result<OrgUnitTypeDto>> GetOrgUnitTypeByIdAsync(Guid id)
        {
            var orgUnitType = await _unitOfWork.OrgUnitTypeRepository.FindAsync(x => x.Id == id);
            if (orgUnitType == null)
                return Result<OrgUnitTypeDto>.Failure(OrganizationErrors.OrgUnitTypeNotFound);

            return Result<OrgUnitTypeDto>.Success(new OrgUnitTypeDto(orgUnitType.Id, orgUnitType.Code, orgUnitType.Name, orgUnitType.LevelOrder, orgUnitType.CanHaveChild));
        }

        public async Task<Result<Guid>> CreateOrgUnitTypeAsync(CreateOrgUnitTypeDto dto)
        {
            var result = OrgUnitType.Create(dto.Code, dto.Name, dto.LevelOrder, dto.CanHaveChild);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.OrgUnitTypeRepository.AddAsync(entity!);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity!.Id);
        }

        public async Task<Result<bool>> UpdateOrgUnitTypeAsync(UpdateOrgUnitTypeDto dto)
        {
            var entity = await _unitOfWork.OrgUnitTypeRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitTypeNotFound);

            entity.UpdateDetails(dto.Code, dto.Name);
            entity.UpdateHierarchyRules(dto.LevelOrder, dto.CanHaveChild);

            _unitOfWork.OrgUnitTypeRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteOrgUnitTypeAsync(Guid id)
        {
            var entity = await _unitOfWork.OrgUnitTypeRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitTypeNotFound);

            _unitOfWork.OrgUnitTypeRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // OrgUnit
        public async Task<Result<IEnumerable<OrgUnitDto>>> GetAllOrgUnitsAsync()
        {
            var orgUnits = await _unitOfWork.OrgUnitRepository.GetAllAsync(new[] { "OrgUnitType", "Parent" });
            var dtoList = orgUnits.Select(x => new OrgUnitDto(
                x.Id, x.Name, x.Code,
                x.OrgUnitTypeId, x.OrgUnitType?.Name ?? "",
                x.ParentId, x.Parent?.Name,
                x.GovernorateId, null, x.IsActive));
            return Result<IEnumerable<OrgUnitDto>>.Success(dtoList);
        }

        public async Task<Result<OrgUnitDto>> GetOrgUnitByIdAsync(Guid id)
        {
            var orgUnit = await _unitOfWork.OrgUnitRepository.FindAsync(x => x.Id == id, new[] { "OrgUnitType", "Parent" });
            if (orgUnit == null)
                return Result<OrgUnitDto>.Failure(OrganizationErrors.OrgUnitNotFound);

            return Result<OrgUnitDto>.Success(new OrgUnitDto(
                orgUnit.Id, orgUnit.Name, orgUnit.Code,
                orgUnit.OrgUnitTypeId, orgUnit.OrgUnitType?.Name ?? "",
                orgUnit.ParentId, orgUnit.Parent?.Name,
                orgUnit.GovernorateId, null, orgUnit.IsActive));
        }

        public async Task<Result<Guid>> CreateOrgUnitAsync(CreateOrgUnitDto dto)
        {
            var result = OrgUnit.Create(dto.Name, dto.Code, dto.OrgUnitTypeId, dto.ParentId, dto.GovernorateId);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.OrgUnitRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateOrgUnitAsync(UpdateOrgUnitDto dto)
        {
            var entity = await _unitOfWork.OrgUnitRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            entity.UpdateDetails(dto.Name, dto.Code);

            _unitOfWork.OrgUnitRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteOrgUnitAsync(Guid id)
        {
            var entity = await _unitOfWork.OrgUnitRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            _unitOfWork.OrgUnitRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // LeadershipPosition
        public async Task<Result<IEnumerable<LeadershipPositionDto>>> GetAllLeadershipPositionsAsync()
        {
            var positions = await _unitOfWork.LeadershipPositionRepository.GetAllAsync(new[] { "OrgUnit", "JobTitle" });
            var dtoList = positions.Select(x => new LeadershipPositionDto(x.Id, x.OrgUnitId, x.OrgUnit?.Name ?? "", x.JobTitleId, x.JobTitle?.Name ?? "", x.Description, x.IsActive, x.JobTitle?.Name ?? ""));
            return Result<IEnumerable<LeadershipPositionDto>>.Success(dtoList);
        }

        public async Task<Result<LeadershipPositionDto>> GetLeadershipPositionByIdAsync(Guid id)
        {
            var position = await _unitOfWork.LeadershipPositionRepository.FindAsync(x => x.Id == id, new[] { "OrgUnit", "JobTitle" });
            if (position == null)
                return Result<LeadershipPositionDto>.Failure(OrganizationErrors.LeadershipPositionNotFound);

            return Result<LeadershipPositionDto>.Success(new LeadershipPositionDto(position.Id, position.OrgUnitId, position.OrgUnit?.Name ?? "", position.JobTitleId, position.JobTitle?.Name ?? "", position.Description, position.IsActive, position.JobTitle?.Name ?? ""));
        }

        public async Task<Result<Guid>> CreateLeadershipPositionAsync(CreateLeadershipPositionDto dto)
        {
            var result = LeadershipPosition.Create(dto.OrgUnitId, dto.JobTitleId, dto.Description);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.LeadershipPositionRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateLeadershipPositionAsync(UpdateLeadershipPositionDto dto)
        {
            var entity = await _unitOfWork.LeadershipPositionRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.LeadershipPositionNotFound);

            _unitOfWork.LeadershipPositionRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteLeadershipPositionAsync(Guid id)
        {
            var entity = await _unitOfWork.LeadershipPositionRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.LeadershipPositionNotFound);

            _unitOfWork.LeadershipPositionRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // LeadershipPositionHistory
        public async Task<Result<IEnumerable<LeadershipPositionHistoryDto>>> GetAllLeadershipPositionHistoriesAsync()
        {
            var histories = await _unitOfWork.LeadershipPositionHistoryRepository.GetAllAsync();
            var dtoList = histories.Select(x => new LeadershipPositionHistoryDto(x.Id, x.LeadershipPositionId, x.EmployeeId, x.StartDate, x.EndDate, x.DecisionNumber, x.DecisionDate, x.Notes));
            return Result<IEnumerable<LeadershipPositionHistoryDto>>.Success(dtoList);
        }

        public async Task<Result<LeadershipPositionHistoryDto>> GetLeadershipPositionHistoryByIdAsync(Guid id)
        {
            var history = await _unitOfWork.LeadershipPositionHistoryRepository.FindAsync(x => x.Id == id);
            if (history == null)
                return Result<LeadershipPositionHistoryDto>.Failure(OrganizationErrors.LeadershipPositionHistoryNotFound);

            return Result<LeadershipPositionHistoryDto>.Success(new LeadershipPositionHistoryDto(history.Id, history.LeadershipPositionId, history.EmployeeId, history.StartDate, history.EndDate, history.DecisionNumber, history.DecisionDate, history.Notes));
        }

        public async Task<Result<Guid>> CreateLeadershipPositionHistoryAsync(CreateLeadershipPositionHistoryDto dto)
        {
            var result = LeadershipPositionHistory.Create(dto.LeadershipPositionId, dto.EmployeeId, dto.StartDate, dto.EndDate, dto.DecisionNumber, dto.DecisionDate, dto.Notes);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.LeadershipPositionHistoryRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateLeadershipPositionHistoryAsync(UpdateLeadershipPositionHistoryDto dto)
        {
            var entity = await _unitOfWork.LeadershipPositionHistoryRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.LeadershipPositionHistoryNotFound);

            _unitOfWork.LeadershipPositionHistoryRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteLeadershipPositionHistoryAsync(Guid id)
        {
            var entity = await _unitOfWork.LeadershipPositionHistoryRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.LeadershipPositionHistoryNotFound);

            _unitOfWork.LeadershipPositionHistoryRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // QualitativeGroup
        public async Task<Result<IEnumerable<QualitativeGroupDto>>> GetAllQualitativeGroupsAsync()
        {
            var groups = await _unitOfWork.QualitativeGroupRepository.GetAllAsync();
            var dtoList = groups.Select(x => new QualitativeGroupDto(x.Id, x.Code, x.Name, x.Description, x.IsActive));
            return Result<IEnumerable<QualitativeGroupDto>>.Success(dtoList);
        }

        public async Task<Result<QualitativeGroupDto>> GetQualitativeGroupByIdAsync(Guid id)
        {
            var group = await _unitOfWork.QualitativeGroupRepository.FindAsync(x => x.Id == id);
            if (group == null)
                return Result<QualitativeGroupDto>.Failure(OrganizationErrors.OrgUnitNotFound);

            return Result<QualitativeGroupDto>.Success(new QualitativeGroupDto(group.Id, group.Code, group.Name, group.Description, group.IsActive));
        }

        public async Task<Result<Guid>> CreateQualitativeGroupAsync(CreateQualitativeGroupDto dto)
        {
            var result = QualitativeGroup.Create(dto.Code, dto.Name, dto.Description);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.QualitativeGroupRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateQualitativeGroupAsync(UpdateQualitativeGroupDto dto)
        {
            var entity = await _unitOfWork.QualitativeGroupRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            entity.UpdateDetails(dto.Code, dto.Name, dto.Description);

            _unitOfWork.QualitativeGroupRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteQualitativeGroupAsync(Guid id)
        {
            var entity = await _unitOfWork.QualitativeGroupRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            _unitOfWork.QualitativeGroupRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // FunctionalGroup
        public async Task<Result<IEnumerable<FunctionalGroupDto>>> GetAllFunctionalGroupsAsync()
        {
            var groups = await _unitOfWork.FunctionalGroupRepository.GetAllAsync(new[] { "QualitativeGroup" });
            var dtoList = groups.Select(x => new FunctionalGroupDto(x.Id, x.QualitativeGroupId, x.QualitativeGroup?.Name ?? "", x.Code, x.Name, x.Description, x.IsActive));
            return Result<IEnumerable<FunctionalGroupDto>>.Success(dtoList);
        }

        public async Task<Result<FunctionalGroupDto>> GetFunctionalGroupByIdAsync(Guid id)
        {
            var group = await _unitOfWork.FunctionalGroupRepository.FindAsync(x => x.Id == id, new[] { "QualitativeGroup" });
            if (group == null)
                return Result<FunctionalGroupDto>.Failure(OrganizationErrors.OrgUnitNotFound);

            return Result<FunctionalGroupDto>.Success(new FunctionalGroupDto(group.Id, group.QualitativeGroupId, group.QualitativeGroup?.Name ?? "", group.Code, group.Name, group.Description, group.IsActive));
        }

        public async Task<Result<Guid>> CreateFunctionalGroupAsync(CreateFunctionalGroupDto dto)
        {
            var result = FunctionalGroup.Create(dto.QualitativeGroupId, dto.Code, dto.Name, dto.Description);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.FunctionalGroupRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateFunctionalGroupAsync(UpdateFunctionalGroupDto dto)
        {
            var entity = await _unitOfWork.FunctionalGroupRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            entity.UpdateDetails(dto.Code, dto.Name, dto.Description);

            _unitOfWork.FunctionalGroupRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteFunctionalGroupAsync(Guid id)
        {
            var entity = await _unitOfWork.FunctionalGroupRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            _unitOfWork.FunctionalGroupRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // JobTitle
        public async Task<Result<IEnumerable<JobTitleDto>>> GetAllJobTitlesAsync()
        {
            var jobTitles = await _unitOfWork.JobTitleRepository.GetAllAsync(new[] { "FunctionalGroup" });
            var dtoList = jobTitles.Select(x => new JobTitleDto(x.Id, x.FunctionalGroupId, x.FunctionalGroup?.Name ?? "", x.Code, x.Name, x.Description, x.IsActive));
            return Result<IEnumerable<JobTitleDto>>.Success(dtoList);
        }

        public async Task<Result<JobTitleDto>> GetJobTitleByIdAsync(Guid id)
        {
            var jobTitle = await _unitOfWork.JobTitleRepository.FindAsync(x => x.Id == id, new[] { "FunctionalGroup" });
            if (jobTitle == null)
                return Result<JobTitleDto>.Failure(OrganizationErrors.OrgUnitNotFound);

            return Result<JobTitleDto>.Success(new JobTitleDto(jobTitle.Id, jobTitle.FunctionalGroupId, jobTitle.FunctionalGroup?.Name ?? "", jobTitle.Code, jobTitle.Name, jobTitle.Description, jobTitle.IsActive));
        }

        public async Task<Result<Guid>> CreateJobTitleAsync(CreateJobTitleDto dto)
        {
            var result = JobTitle.Create(dto.FunctionalGroupId, dto.Code, dto.Name, dto.Description);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.JobTitleRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateJobTitleAsync(UpdateJobTitleDto dto)
        {
            var entity = await _unitOfWork.JobTitleRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            entity.UpdateDetails(dto.Code, dto.Name, dto.Description);

            _unitOfWork.JobTitleRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteJobTitleAsync(Guid id)
        {
            var entity = await _unitOfWork.JobTitleRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            _unitOfWork.JobTitleRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        // JobGrade
        public async Task<Result<IEnumerable<JobGradeDto>>> GetAllJobGradesAsync()
        {
            var jobGrades = await _unitOfWork.JobGradeRepository.GetAllAsync();
            var dtoList = jobGrades.Select(x => new JobGradeDto(x.Id, x.Code, x.Name, x.GradeLevel, x.Description, x.YearsNo, x.IsActive));
            return Result<IEnumerable<JobGradeDto>>.Success(dtoList);
        }

        public async Task<Result<JobGradeDto>> GetJobGradeByIdAsync(Guid id)
        {
            var jobGrade = await _unitOfWork.JobGradeRepository.FindAsync(x => x.Id == id);
            if (jobGrade == null)
                return Result<JobGradeDto>.Failure(OrganizationErrors.OrgUnitNotFound);

            return Result<JobGradeDto>.Success(new JobGradeDto(jobGrade.Id, jobGrade.Code, jobGrade.Name, jobGrade.GradeLevel, jobGrade.Description, jobGrade.YearsNo, jobGrade.IsActive));
        }

        public async Task<Result<Guid>> CreateJobGradeAsync(CreateJobGradeDto dto)
        {
            var result = JobGrade.Create(dto.Code, dto.Name, dto.GradeLevel, dto.Description, dto.YearsNo);
            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            var entity = result.Value;
            await _unitOfWork.JobGradeRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(entity.Id);
        }

        public async Task<Result<bool>> UpdateJobGradeAsync(UpdateJobGradeDto dto)
        {
            var entity = await _unitOfWork.JobGradeRepository.FindAsync(x => x.Id == dto.Id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            entity.UpdateDetails(dto.Code, dto.Name, dto.GradeLevel, dto.Description, dto.YearsNo);

            _unitOfWork.JobGradeRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteJobGradeAsync(Guid id)
        {
            var entity = await _unitOfWork.JobGradeRepository.FindAsync(x => x.Id == id);
            if (entity == null)
                return Result<bool>.Failure(OrganizationErrors.OrgUnitNotFound);

            _unitOfWork.JobGradeRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}