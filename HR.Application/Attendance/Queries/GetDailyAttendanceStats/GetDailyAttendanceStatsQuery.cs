using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Queries.GetDailyAttendanceStats
{
    public record GetDailyAttendanceStatsQuery(DateTime? Date) : IQuery<DailyAttendanceStatsResponse>;
}
