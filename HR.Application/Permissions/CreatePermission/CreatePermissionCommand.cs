using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Permissions.CreatePermission
{
    public record CreatePermissionCommand(
        Guid EmployeeId,
        PermissionType PermissionType,
        DateTime Date,
        TimeSpan FromTime,
        TimeSpan ToTime,
        string? Notes
    ) : ICommand<Guid>;
}
