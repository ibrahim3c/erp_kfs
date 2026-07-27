using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.CreateKpiReport
{
    public record CreateKpiReportCommand(
        Guid EmployeeId,
        int Year,
        decimal EfficiencyScore,
        decimal DisciplineScore,
        decimal AchievementScore,
        Guid? EvaluatorId,
        string Status,
        string? Notes
    ) : ICommand<Guid>;
}
