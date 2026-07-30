using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.GetSpecialLeaveRequests
{
    public sealed class GetSpecialLeaveRequestsQueryHandler
        : IQueryHandler<GetSpecialLeaveRequestsQuery, List<GetSpecialLeaveRequestsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetSpecialLeaveRequestsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetSpecialLeaveRequestsResponse>>> Handle(
            GetSpecialLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    lr.Id,
                    e.Name AS EmployeeName,
                    CASE lr.LeaveCategory
                        WHEN 'Maternity' THEN 'أجازة وضع'
                        WHEN 'Hajj' THEN 'أجازة حج'
                        WHEN 'ChildCare' THEN 'رعاية طفل'
                        WHEN 'Exam' THEN 'أداء امتحانات'
                        ELSE lr.LeaveCategory
                    END AS LeaveCategoryName,
                    lr.StartDate,
                    lr.EndDate,
                    CASE lr.SalaryStatus
                        WHEN 'FullPay' THEN 'أجر كامل'
                        WHEN 'PartialPay' THEN 'أجر جزئي'
                        WHEN 'NoPay' THEN 'بدون أجر'
                        ELSE lr.SalaryStatus
                    END AS SalaryStatusName,
                    lr.AttachmentPath,
                    CASE lr.Status
                        WHEN 'Pending' THEN 'قيد المراجعة'
                        WHEN 'Approved' THEN 'معتمدة'
                        WHEN 'Rejected' THEN 'مرفوضة'
                        WHEN 'Cancelled' THEN 'سارية'
                        ELSE lr.Status
                    END AS Status
                FROM HR.LeaveRequests lr
                INNER JOIN HR.Employees e ON e.Id = lr.EmployeeId
                WHERE lr.LeaveCategory IN ('Maternity', 'Hajj', 'ChildCare', 'Exam')
                ORDER BY lr.CreatedAt DESC
                """;

            var response = (await connection.QueryAsync<GetSpecialLeaveRequestsResponse>(sql)).ToList();

            return Result<List<GetSpecialLeaveRequestsResponse>>.Success(response);
        }
    }
}
