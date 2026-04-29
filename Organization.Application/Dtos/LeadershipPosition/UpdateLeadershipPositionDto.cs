namespace Organization.Application.Dtos.LeadershipPosition
{
    public record UpdateLeadershipPositionDto(Guid Id, Guid OrgUnitId, Guid JobTitleId, string Description);
}