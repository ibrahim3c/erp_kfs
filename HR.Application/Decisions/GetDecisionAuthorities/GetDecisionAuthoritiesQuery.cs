using Modules.Shared.Application.Messaging;

namespace HR.Application.Decisions.GetDecisionAuthorities
{
    public record GetDecisionAuthoritiesQuery() : IQuery<List<GetDecisionAuthorityResponse>>;
}
