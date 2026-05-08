namespace HR.Application.Attendance.Queries.GetDailyAttendance
{
    public class DailyAttendanceResponse
    {
        public int TotalWorkforce { get; init; }
        public int PresentCount { get; init; }
        public int LateCount { get; init; }
        public int AbsentOrVacationCount { get; init; }
        public DateTime CurrentDate { get; init; }
        public List<AttendanceRowDto> Items { get; init; } = new();
    }
}
