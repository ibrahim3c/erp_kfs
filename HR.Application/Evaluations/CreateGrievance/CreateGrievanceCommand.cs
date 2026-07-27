using Modules.Shared.Application.Messaging;

namespace HR.Application.Evaluations.CreateGrievance
{
    public record CreateGrievanceCommand(
        Guid EmployeeId,
        string GrievanceType,
        string ComplainedDecisionNumber,
        DateTime ComplainedDecisionDate,
        DateTime SubmissionDate,
        string Reasons,
        string? AttachmentPath
    ) : ICommand<Guid>;
}
