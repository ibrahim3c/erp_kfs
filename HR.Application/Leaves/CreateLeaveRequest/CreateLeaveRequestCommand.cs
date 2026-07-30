using HR.Domain.Leaves;
using Modules.Shared.Application.Messaging;

namespace HR.Application.Leaves.CreateLeaveRequest
{
    public record CreateLeaveRequestCommand(
        Guid EmployeeId,
        LeaveCategory LeaveCategory,
        DateTime StartDate,
        DateTime EndDate,
        Guid? ReplacementEmployeeId,
        string? ContactInfo,
        string? ReportAuthority,
        string? DecisionNumber,
        string? Diagnosis,
        string? ChildName,
        DateTime? ChildDateOfBirth,
        string? AttachmentPath,
        string? Notes,
        decimal? PayPercentage
    ) : ICommand<Guid>;
}
