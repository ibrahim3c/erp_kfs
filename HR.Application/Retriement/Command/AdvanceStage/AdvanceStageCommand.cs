using HR.Domain.Retirement.Enums;
using MediatR;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Retriement.Command.AdvanceStage
{
    public record AdvanceStageCommand(Guid RetirementFileId, RetirementStage NextStage) : ICommand;
}
