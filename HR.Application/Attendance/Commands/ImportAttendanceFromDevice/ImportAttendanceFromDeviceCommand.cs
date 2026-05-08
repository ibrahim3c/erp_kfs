using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Commands.ImportAttendanceFromDevice
{
    public record ImportAttendanceFromDeviceCommand(
        List<DeviceRecordDto> Records
    ) : ICommand<int>;
}
