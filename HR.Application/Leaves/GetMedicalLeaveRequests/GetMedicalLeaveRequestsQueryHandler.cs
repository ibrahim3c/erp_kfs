using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.GetMedicalLeaveRequests
{
    public sealed class GetMedicalLeaveRequestsQueryHandler
        : IQueryHandler<GetMedicalLeaveRequestsQuery, List<GetMedicalLeaveRequestsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetMedicalLeaveRequestsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetMedicalLeaveRequestsResponse>>> Handle(
            GetMedicalLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    lr.Id,
                    e.Name AS EmployeeName,
                    lr.Diagnosis,
                    lr.StartDate,
                    lr.EndDate,
                    lr.DurationDays,
                    lr.PayPercentage,
                    lr.AttachmentPath,
                    CASE lr.Status
                        WHEN 'Pending' THEN 'جديدة'
                        WHEN 'Approved' THEN 'معتمدة'
                        WHEN 'Rejected' THEN 'مرفوضة'
                        WHEN 'Cancelled' THEN 'منتهية'
                        ELSE lr.Status
                    END AS Status
                FROM HR.LeaveRequests lr
                INNER JOIN HR.Employees e ON e.Id = lr.EmployeeId
                WHERE lr.LeaveCategory = 'Medical'
                ORDER BY lr.CreatedAt DESC
                """;

            var response = (await connection.QueryAsync<GetMedicalLeaveRequestsResponse>(sql)).ToList();

            return Result<List<GetMedicalLeaveRequestsResponse>>.Success(response);
        }
    }
}
