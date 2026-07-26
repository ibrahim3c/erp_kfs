using Modules.Shared.Application.Messaging;

namespace HR.Application.Decisions.GetDecisionTypes
{
    public record GetDecisionTypesQuery() : IQuery<List<GetDecisionTypeResponse>>;
}
