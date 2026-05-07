using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Commands.ConvertLateToPermission
{
    public record ConvertLateToPermissionCommand(
        Guid AttendanceRecordId,
        PermissionType PermissionType,
        DateTime Date,
        TimeSpan FromTime,
        TimeSpan ToTime,
        string? Notes
    ) : ICommand<Guid>;
}
