namespace HR.Application.Evaluations.GetGrievanceList
{
    public class GetGrievanceListResponse
    {
        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string GrievanceTypeName { get; init; } = string.Empty;
        public string ComplainedDecisionNumber { get; init; } = string.Empty;
        public DateTime ComplainedDecisionDate { get; init; }
        public DateTime SubmissionDate { get; init; }
        public string Reasons { get; init; } = string.Empty;
        public bool HasAttachment { get; init; }
        public string StatusName { get; init; } = string.Empty;
        public string? CommitteeNotes { get; init; }
        public DateTime? ResolutionDate { get; init; }
    }
}
