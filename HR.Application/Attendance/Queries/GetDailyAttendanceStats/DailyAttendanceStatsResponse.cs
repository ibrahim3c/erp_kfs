namespace HR.Application.Attendance.Queries.GetDailyAttendanceStats
{
    public class DailyAttendanceStatsResponse
    {
        public int TotalWorkforce { get; init; }
        public int PresentCount { get; init; }
        public int LateCount { get; init; }
        public int AbsentCount { get; init; }
        public int MissionCount { get; init; }
        public int VacationCount { get; init; }
        public int PermissionCount { get; init; }
    }
}
