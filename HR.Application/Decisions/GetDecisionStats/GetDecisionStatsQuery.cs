using Modules.Shared.Application.Messaging;

namespace HR.Application.Decisions.GetDecisionStats
{
    public record GetDecisionStatsQuery() : IQuery<GetDecisionStatsResponse>;
}
