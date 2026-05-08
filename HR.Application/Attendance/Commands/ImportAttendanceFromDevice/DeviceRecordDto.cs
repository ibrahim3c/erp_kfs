namespace HR.Application.Attendance.Commands.ImportAttendanceFromDevice
{
    public class DeviceRecordDto
    {
        public Guid EmployeeId { get; init; }
        public DateTime Date { get; init; }
        public TimeSpan Time { get; init; }
        public string Direction { get; init; } = string.Empty;
    }
}
