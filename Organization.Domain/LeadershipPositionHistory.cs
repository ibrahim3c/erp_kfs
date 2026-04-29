using Modules.Shared.Domain;
namespace Organization.Domain;

public sealed class LeadershipPositionHistory : Entity
{
    private LeadershipPositionHistory() { }

    private LeadershipPositionHistory(
        Guid id,
        Guid leadershipPositionId,
        Guid employeeId,
        DateTime startDate,
        DateTime? endDate,
        string decisionNumber,
        DateTime? decisionDate,
        string notes) : base(id)
    {
        LeadershipPositionId = leadershipPositionId;
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        DecisionNumber = decisionNumber;
        DecisionDate = decisionDate;
        Notes = notes;
    }

    public Guid LeadershipPositionId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string DecisionNumber { get; private set; }
    public DateTime? DecisionDate { get; private set; }
    public string Notes { get; private set; }

    // Navigation Properties
    public LeadershipPosition? LeadershipPosition { get; private set; }

    // Factory method
    public static Result<LeadershipPositionHistory> Create(
        Guid leadershipPositionId,
        Guid employeeId,
        DateTime startDate,
        DateTime? endDate = null,
        string decisionNumber = null,
        DateTime? decisionDate = null,
        string notes = null)
    {
        if (leadershipPositionId == Guid.Empty)
            return Result<LeadershipPositionHistory>.Failure(OrganizationErrors.OrgUnitIdEmpty);

        if (employeeId == Guid.Empty)
            return Result<LeadershipPositionHistory>.Failure(OrganizationErrors.JobTitleIdEmpty);

        if (endDate.HasValue && endDate < startDate)
            return Result<LeadershipPositionHistory>.Failure(OrganizationErrors.EndDateInvalid);

        var history = new LeadershipPositionHistory(
            Guid.NewGuid(),
            leadershipPositionId,
            employeeId,
            startDate,
            endDate,
            decisionNumber,
            decisionDate,
            notes
        );

        return Result<LeadershipPositionHistory>.Success(history);
    }

    // Business behavior: End the leadership period
    public Result EndPosition(DateTime endDate)
    {
        if (endDate < StartDate)
            return Result.Failure(OrganizationErrors.EndDateInvalid);

        EndDate = endDate;
        return Result.Success();
    }
}
