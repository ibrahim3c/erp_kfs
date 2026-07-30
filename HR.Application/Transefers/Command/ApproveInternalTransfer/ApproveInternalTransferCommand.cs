

using Modules.Shared.Application.Messaging;

namespace HR.Application.Transefers.Command.ApproveInternalTransfer
{
    public record ApproveInternalTransferCommand(Guid TransferId) : ICommand;
}
