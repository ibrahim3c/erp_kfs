using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.ResolveGrievance
{
    public record ResolveGrievanceCommand(
        Guid GrievanceId,
        string NewStatus,
        string? CommitteeNotes,
        DateTime ResolutionDate
    ) : ICommand<Guid>;
}
