namespace HR.Application.Decisions.GetDecisionStats
{
    public class GetDecisionStatsResponse
    {
        public int TotalDecisions { get; init; }
        public int PendingExecution { get; init; }
        public int NewAppointments { get; init; }
        public int Promotions { get; init; }
    }
}
