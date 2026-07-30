namespace HR.Application.Evaluations.GetKpiReportList
{
    public class GetKpiReportListResponse
    {
        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string? JobGradeName { get; init; }
        public int Year { get; init; }
        public decimal Score { get; init; }
        public decimal EfficiencyScore { get; init; }
        public decimal DisciplineScore { get; init; }
        public decimal AchievementScore { get; init; }
        public string Grade { get; init; } = string.Empty;
        public string? EvaluatorName { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
