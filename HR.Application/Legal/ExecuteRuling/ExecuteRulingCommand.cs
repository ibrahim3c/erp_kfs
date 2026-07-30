using Modules.Shared.Application.Messaging;

namespace HR.Application.Legal.ExecuteRuling
{
    public record ExecuteRulingCommand(
        Guid RulingId,
        Guid DecisionId
    ) : ICommand;
}
