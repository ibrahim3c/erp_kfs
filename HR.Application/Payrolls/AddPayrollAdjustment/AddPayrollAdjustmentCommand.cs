using HR.Domain.Payrolls;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Payrolls.AddPayrollAdjustment
{
    public record AddPayrollAdjustmentCommand(
        Guid EntryId,
        AdjustmentType Type,
        decimal Amount,
        string Reason
    ) : ICommand;
}
