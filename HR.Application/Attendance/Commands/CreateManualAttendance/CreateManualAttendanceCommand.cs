using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Commands.CreateManualAttendance
{
    public record CreateManualAttendanceCommand(
        Guid EmployeeId,
        DateTime Date,
        MovementType MovementType,
        TimeSpan Time,
        string? Notes
    ) : ICommand<Guid>;
}
