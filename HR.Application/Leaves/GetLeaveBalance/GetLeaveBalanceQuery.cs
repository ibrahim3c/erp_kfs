using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.GetLeaveBalance
{
    public record GetLeaveBalanceQuery(Guid EmployeeId) : IQuery<GetLeaveBalanceResponse>;
}
