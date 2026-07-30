using Modules.Shared.Application.Messaging;

namespace HR.Application.Funds.GetFundClaims
{
    public record GetFundClaimsQuery() : IQuery<List<GetFundClaimsResponse>>;
}
