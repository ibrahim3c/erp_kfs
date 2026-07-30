using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.GetSpecialLeaveRequests
{
    public record GetSpecialLeaveRequestsQuery() : IQuery<List<GetSpecialLeaveRequestsResponse>>;
}
