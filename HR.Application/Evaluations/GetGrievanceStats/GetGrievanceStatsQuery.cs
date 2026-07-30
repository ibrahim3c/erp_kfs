using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.GetGrievanceStats
{
    public record GetGrievanceStatsQuery() : IQuery<GetGrievanceStatsResponse>;
}
