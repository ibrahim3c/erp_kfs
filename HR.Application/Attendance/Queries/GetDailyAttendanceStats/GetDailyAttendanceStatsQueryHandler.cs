using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Queries.GetDailyAttendanceStats
{
    public sealed class GetDailyAttendanceStatsQueryHandler
        : IQueryHandler<GetDailyAttendanceStatsQuery, DailyAttendanceStatsResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetDailyAttendanceStatsQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<DailyAttendanceStatsResponse>> Handle(
            GetDailyAttendanceStatsQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            var date = request.Date?.Date ?? DateTime.UtcNow.Date;

            var totalWorkforce = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.Employees WHERE IsActive = 1");

            var presentCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 1",
                new { Date = date });

            var lateCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 2",
                new { Date = date });

            var missionCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 4",
                new { Date = date });

            var vacationCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 5",
                new { Date = date });

            var permissionCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM HR.AttendanceRecords WHERE Date = @Date AND Status = 6",
                new { Date = date });

            var recorded = presentCount + lateCount + missionCount + vacationCount + permissionCount;
            var absentCount = totalWorkforce - recorded;
            if (absentCount < 0) absentCount = 0;

            var response = new DailyAttendanceStatsResponse
            {
                TotalWorkforce = totalWorkforce,
                PresentCount = presentCount,
                LateCount = lateCount,
                AbsentCount = absentCount,
                MissionCount = missionCount,
                VacationCount = vacationCount,
                PermissionCount = permissionCount
            };

            return Result<DailyAttendanceStatsResponse>.Success(response);
        }
    }
}
