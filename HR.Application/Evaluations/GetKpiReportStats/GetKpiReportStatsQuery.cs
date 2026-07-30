using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.GetKpiReportStats
{
    public record GetKpiReportStatsQuery(int? Year) : IQuery<GetKpiReportStatsResponse>;
}
