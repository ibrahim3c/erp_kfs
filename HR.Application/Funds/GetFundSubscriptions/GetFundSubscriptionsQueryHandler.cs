using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Funds.GetFundSubscriptions
{
    public sealed class GetFundSubscriptionsQueryHandler
        : IQueryHandler<GetFundSubscriptionsQuery, List<GetFundSubscriptionsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFundSubscriptionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetFundSubscriptionsResponse>>> Handle(
            GetFundSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    fs.Id,
                    fs.EmployeeId,
                    e.Name AS EmployeeName,
                    fs.SubscriptionDate,
                    CASE fs.FundType
                        WHEN 'Fellowship' THEN 'زمالة (فقط)'
                        WHEN 'SocialSolidarity' THEN 'التكافل الاجتماعي'
                        WHEN 'Both' THEN 'زمالة + تكافل'
                        ELSE fs.FundType
                    END AS FundTypeName,
                    fs.DeductionAmount,
                    DATEDIFF(MONTH, fs.SubscriptionDate, GETDATE()) * fs.DeductionAmount AS TotalPaid,
                    CASE fs.Status
                        WHEN 'Active' THEN 'نشط'
                        WHEN 'Suspended' THEN 'معلق'
                        WHEN 'Withdrawn' THEN 'غير مشترك'
                        ELSE fs.Status
                    END AS Status
                FROM HR.FundSubscriptions fs
                INNER JOIN HR.Employees e ON e.Id = fs.EmployeeId
                ORDER BY fs.SubscriptionDate DESC
                """;

            var response = (await connection.QueryAsync<GetFundSubscriptionsResponse>(sql)).ToList();

            return Result<List<GetFundSubscriptionsResponse>>.Success(response);
        }
    }
}
