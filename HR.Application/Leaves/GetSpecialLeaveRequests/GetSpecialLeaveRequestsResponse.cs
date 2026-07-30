namespace HR.Application.Leaves.GetSpecialLeaveRequests
{
    public class GetSpecialLeaveRequestsResponse
    {
        public Guid Id { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string LeaveCategoryName { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string SalaryStatusName { get; init; } = string.Empty;
        public string? AttachmentPath { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
