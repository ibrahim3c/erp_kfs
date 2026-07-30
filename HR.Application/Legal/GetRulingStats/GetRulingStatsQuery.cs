using Modules.Shared.Application.Messaging;

namespace HR.Application.Legal.GetRulingStats
{
    public record GetRulingStatsQuery() : IQuery<GetRulingStatsResponse>;
}
