namespace Organization.Application.Dtos.LeadershipPositionHistory
{
    public record CreateLeadershipPositionHistoryDto(
        Guid LeadershipPositionId,
        Guid EmployeeId,
        DateTime StartDate,
        DateTime? EndDate,
        string DecisionNumber,
        DateTime? DecisionDate,
        string Notes);
}