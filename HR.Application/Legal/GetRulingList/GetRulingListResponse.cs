namespace HR.Application.Legal.GetRulingList
{
    public class GetRulingListResponse
    {
        public Guid Id { get; init; }
        public string CaseNumber { get; init; } = string.Empty;
        public string Year { get; init; } = string.Empty;
        public string EmployeeName { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string ExecutionType { get; init; } = string.Empty;
        public string? AttachmentPath { get; init; }
        public string Status { get; init; } = string.Empty;
        public Guid? DecisionId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
