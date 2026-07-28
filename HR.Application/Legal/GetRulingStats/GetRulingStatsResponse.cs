namespace HR.Application.Legal.GetRulingStats
{
    public class GetRulingStatsResponse
    {
        public int PendingCount { get; init; }
        public int ExecutedCount { get; init; }
        public int InProgressCount { get; init; }
        public int TotalCount { get; init; }
    }
}
