namespace Organization.Application.Dtos.LeadershipPositionHistory
{
    public record LeadershipPositionHistoryDto(
        Guid Id,
        Guid LeadershipPositionId,
        Guid EmployeeId,
        DateTime StartDate,
        DateTime? EndDate,
        string DecisionNumber,
        DateTime? DecisionDate,
        string Notes);
}