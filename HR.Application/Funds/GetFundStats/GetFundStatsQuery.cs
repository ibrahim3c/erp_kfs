using Modules.Shared.Application.Messaging;

namespace HR.Application.Funds.GetFundStats
{
    public record GetFundStatsQuery() : IQuery<GetFundStatsResponse>;
}
