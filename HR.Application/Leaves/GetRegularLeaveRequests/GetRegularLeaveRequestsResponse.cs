namespace HR.Application.Leaves.GetRegularLeaveRequests
{
    public class GetRegularLeaveRequestsResponse
    {
        public Guid Id { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string LeaveCategoryName { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int DurationDays { get; init; }
        public string? ReplacementEmployeeName { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
