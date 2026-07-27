namespace HR.Application.Funds.GetFundClaims
{
    public class GetFundClaimsResponse
    {
        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string ClaimTypeName { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public decimal? Amount { get; init; }
        public string? AttachmentPath { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? PaymentOrderNumber { get; init; }
    }
}
