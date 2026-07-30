using Modules.Shared.Application.Messaging;

namespace HR.Application.Decisions.GetDecisionList
{
    public record GetDecisionListQuery() : IQuery<List<GetDecisionListResponse>>;
}
