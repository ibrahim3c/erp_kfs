namespace HR.Application.Funds.GetFundSubscriptions
{
    public class GetFundSubscriptionsResponse
    {
        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public DateTime SubscriptionDate { get; init; }
        public string FundTypeName { get; init; } = string.Empty;
        public decimal DeductionAmount { get; init; }
        public decimal TotalPaid { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
