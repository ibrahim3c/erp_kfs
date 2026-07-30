namespace HR.Application.Funds.GetFundStats
{
    public class GetFundStatsResponse
    {
        public int TotalSubscribers { get; init; }
        public decimal MonthlySubscriptionTotal { get; init; }
        public int PendingClaimsCount { get; init; }
    }
}
