namespace HR.Application.Decisions.GetDecisionList
{
    public class GetDecisionListResponse
    {
        public Guid Id { get; init; }
        public string Number { get; init; } = string.Empty;
        public string? Subject { get; init; }
        public string? TypeName { get; init; }
        public DateTime DecisionDate { get; init; }
        public int EmployeeCount { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? FilePath { get; init; }
    }
}
