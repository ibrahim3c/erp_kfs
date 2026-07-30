using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.GetMedicalLeaveRequests
{
    public record GetMedicalLeaveRequestsQuery() : IQuery<List<GetMedicalLeaveRequestsResponse>>;
}
