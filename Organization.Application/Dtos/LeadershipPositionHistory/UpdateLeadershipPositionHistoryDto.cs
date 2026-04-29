namespace Organization.Application.Dtos.LeadershipPositionHistory
{
    public record UpdateLeadershipPositionHistoryDto(
        Guid Id,
        Guid LeadershipPositionId,
        Guid EmployeeId,
        DateTime StartDate,
        DateTime? EndDate,
        string DecisionNumber,
        DateTime? DecisionDate,
        string Notes);
}