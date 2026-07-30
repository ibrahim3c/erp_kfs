namespace HR.Application.Evaluations.GetGrievanceStats
{
    public class GetGrievanceStatsResponse
    {
        public int TotalGrievances { get; init; }
        public int PendingCount { get; init; }
        public int AcceptedCount { get; init; }
        public int RejectedCount { get; init; }
    }
}
