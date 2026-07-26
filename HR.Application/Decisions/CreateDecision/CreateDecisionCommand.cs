using Modules.Shared.Application.Messaging;

namespace HR.Application.Decisions.CreateDecision
{
    public record CreateDecisionCommand(
        string Number,
        DateTime DecisionDate,
        DateTime? ValidFrom,
        DateTime? ValidTo,
        Guid DecisionTypeId,
        Guid DecisionAuthorityId,
        string? Subject,
        string? Notes,
        string? FilePath,
        bool AffectsEmployee,
        bool AffectsGroup,
        bool IsTemporary,
        Guid[] EmployeeIds
    ) : ICommand<Guid>;
}
