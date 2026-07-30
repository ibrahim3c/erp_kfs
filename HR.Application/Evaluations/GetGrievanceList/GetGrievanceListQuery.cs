using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.GetGrievanceList
{
    public record GetGrievanceListQuery() : IQuery<List<GetGrievanceListResponse>>;
}
