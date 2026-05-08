using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Commands.UpdateAttendance
{
    public record UpdateAttendanceCommand(
        Guid Id,
        TimeSpan? CheckIn,
        TimeSpan? CheckOut,
        string? Notes
    ) : ICommand<Guid>;
}
