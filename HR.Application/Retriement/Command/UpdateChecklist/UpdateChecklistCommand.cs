using MediatR;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Retriement.Command.UpdateChecklist
{
    public sealed record UpdateChecklistCommand(Guid RetirementFileId, bool JoinPeriodsAdded, bool SpecialLeavesReviewed) : ICommand;

}
