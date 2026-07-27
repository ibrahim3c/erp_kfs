using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.GetRegularLeaveRequests
{
    public record GetRegularLeaveRequestsQuery() : IQuery<List<GetRegularLeaveRequestsResponse>>;
}
