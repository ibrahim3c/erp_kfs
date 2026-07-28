using Modules.Shared.Application.Messaging;

namespace HR.Application.Legal.GetRulingList
{
    public record GetRulingListQuery() : IQuery<List<GetRulingListResponse>>;
}
