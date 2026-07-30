using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.RejectLeaveRequest
{
    public record RejectLeaveRequestCommand(Guid LeaveRequestId) : ICommand;
}
