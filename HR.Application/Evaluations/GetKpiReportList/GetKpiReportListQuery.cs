using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.GetKpiReportList
{
    public record GetKpiReportListQuery(int? Year) : IQuery<List<GetKpiReportListResponse>>;
}
