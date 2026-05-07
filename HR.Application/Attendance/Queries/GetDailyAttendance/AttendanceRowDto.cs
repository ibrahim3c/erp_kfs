namespace HR.Application.Attendance.Queries.GetDailyAttendance
{
    public class AttendanceRowDto
    {
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string DepartmentName { get; init; } = string.Empty;
        public string? CheckIn { get; init; }
        public string? CheckOut { get; init; }
        public double WorkedHours { get; init; }
        public int LateMinutes { get; init; }
        public string Status { get; init; } = string.Empty;
        public string StatusClass { get; init; } = string.Empty;
        public string? Notes { get; init; }
    }
}
