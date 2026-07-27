using Modules.Shared.Application.Messaging;

namespace HR.Application.Funds.GetFundSubscriptions
{
    public record GetFundSubscriptionsQuery() : IQuery<List<GetFundSubscriptionsResponse>>;
}
