namespace HR.Application.Leaves.GetMedicalLeaveRequests
{
    public class GetMedicalLeaveRequestsResponse
    {
        public Guid Id { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string? Diagnosis { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int DurationDays { get; init; }
        public decimal? PayPercentage { get; init; }
        public string? AttachmentPath { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
