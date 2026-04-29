namespace Organization.Application.Dtos.LeadershipPosition
{
    public record LeadershipPositionDto(
        Guid Id,
        Guid OrgUnitId,
        string OrgUnitName,
        Guid JobTitleId,
        string JobTitleName,
        string Description,
        bool IsActive,
        string? Name);
}