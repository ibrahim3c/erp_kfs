using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.GetRegularLeaveRequests
{
    public sealed class GetRegularLeaveRequestsQueryHandler
        : IQueryHandler<GetRegularLeaveRequestsQuery, List<GetRegularLeaveRequestsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRegularLeaveRequestsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetRegularLeaveRequestsResponse>>> Handle(
            GetRegularLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    lr.Id,
                    e.Name AS EmployeeName,
                    CASE lr.LeaveCategory
                        WHEN 'Regular' THEN 'اعتيادي'
                        WHEN 'Casual' THEN 'عارضة'
                        ELSE lr.LeaveCategory
                    END AS LeaveCategoryName,
                    lr.StartDate,
                    lr.EndDate,
                    lr.DurationDays,
                    ISNULL(re.Name, 'لا يوجد') AS ReplacementEmployeeName,
                    CASE lr.Status
                        WHEN 'Pending' THEN 'قيد الموافقة'
                        WHEN 'Approved' THEN 'معتمدة'
                        WHEN 'Rejected' THEN 'مرفوضة'
                        WHEN 'Cancelled' THEN 'ملغاة'
                        ELSE lr.Status
                    END AS Status
                FROM HR.LeaveRequests lr
                INNER JOIN HR.Employees e ON e.Id = lr.EmployeeId
                LEFT JOIN HR.Employees re ON re.Id = lr.ReplacementEmployeeId
                WHERE lr.LeaveCategory IN ('Regular', 'Casual')
                ORDER BY lr.CreatedAt DESC
                """;

            var response = (await connection.QueryAsync<GetRegularLeaveRequestsResponse>(sql)).ToList();

            return Result<List<GetRegularLeaveRequestsResponse>>.Success(response);
        }
    }
}
