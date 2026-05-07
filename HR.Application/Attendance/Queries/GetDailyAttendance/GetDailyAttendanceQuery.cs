using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Queries.GetDailyAttendance
{
    public record GetDailyAttendanceQuery(
        DateTime? Date,
        Guid? OrgUnitId,
        string? Status
    ) : IQuery<DailyAttendanceResponse>;
}
