using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Funds.GetFundClaims
{
    public sealed class GetFundClaimsQueryHandler
        : IQueryHandler<GetFundClaimsQuery, List<GetFundClaimsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFundClaimsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetFundClaimsResponse>>> Handle(
            GetFundClaimsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    fc.Id,
                    fc.EmployeeId,
                    e.Name AS EmployeeName,
                    CASE fc.ClaimType
                        WHEN 'RetirementBonus' THEN 'مكافأة نهاية خدمة (بلوغ السن)'
                        WHEN 'MarriageGrant' THEN 'منحة زواج'
                        WHEN 'NewbornGrant' THEN 'منحة مولود'
                        WHEN 'Surgery' THEN 'عملية جراحية (حالة مرضية)'
                        WHEN 'FuneralExpenses' THEN 'مصاريف جنازة'
                        ELSE fc.ClaimType
                    END AS ClaimTypeName,
                    fc.EventDate,
                    fc.Amount,
                    fc.AttachmentPath,
                    CASE fc.Status
                        WHEN 'Pending' THEN 'قيد المراجعة'
                        WHEN 'UnderReview' THEN 'قيد المراجعة'
                        WHEN 'Approved' THEN 'تمت الموافقة'
                        WHEN 'Rejected' THEN 'مرفوض'
                        WHEN 'Paid' THEN 'تم الصرف'
                        ELSE fc.Status
                    END AS Status,
                    fc.PaymentOrderNumber
                FROM HR.FundClaims fc
                INNER JOIN HR.Employees e ON e.Id = fc.EmployeeId
                ORDER BY fc.CreatedAt DESC
                """;

            var response = (await connection.QueryAsync<GetFundClaimsResponse>(sql)).ToList();

            return Result<List<GetFundClaimsResponse>>.Success(response);
        }
    }
}
