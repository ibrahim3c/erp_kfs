using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.ApproveLeaveRequest
{
    public record ApproveLeaveRequestCommand(Guid LeaveRequestId) : ICommand;
}
