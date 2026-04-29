namespace Organization.Application.Dtos.LeadershipPosition
{
    public record CreateLeadershipPositionDto(Guid OrgUnitId, Guid JobTitleId, string Description);
}