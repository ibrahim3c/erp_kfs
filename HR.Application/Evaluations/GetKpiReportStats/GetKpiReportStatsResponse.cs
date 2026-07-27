namespace HR.Application.Evaluations.GetKpiReportStats
{
    public class GetKpiReportStatsResponse
    {
        public int TotalReports { get; init; }
        public int ApprovedCount { get; init; }
        public int DraftCount { get; init; }
        public decimal AverageScore { get; init; }
    }
}
