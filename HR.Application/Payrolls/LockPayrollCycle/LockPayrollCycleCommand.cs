using Modules.Shared.Application.Messaging;


namespace HR.Application.Payrolls.LockPayrollCycle
{
    public record LockPayrollCycleCommand(Guid CycleId) : ICommand;
}
