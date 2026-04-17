using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Loans.GetInsurancePurchaseList
{
    public class GetInsurancePurchaseListHandler : IQueryHandler<GetInsurancePurchaseListQuery, List<GetInsurancePurchaseListResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetInsurancePurchaseListHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<GetInsurancePurchaseListResponse>>> Handle(
            GetInsurancePurchaseListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = """
                SELECT
                    ip."Id"                     AS Id,
                    e."Name"                    AS EmployeeName,
                    ip."InsuranceAuthority"     AS InsuranceAuthority,
                    ip."PurchasedYears"         AS PurchasedYears,
                    ip."TotalCost"              AS TotalCost,
                    ip."MonthlyInstallment"     AS MonthlyInstallment,
                    ip."RemainingAmount"        AS RemainingAmount,
                    ip."DeductionStartDate"     AS DeductionStartDate,
                    ip."Status"                 AS Status
                FROM "HR"."InsurancePeriodPurchases" ip
                INNER JOIN "HR"."Employees" e ON ip."EmployeeId" = e."Id"
                ORDER BY ip."DeductionStartDate" DESC;
                """;

            var result = (await connection.QueryAsync<GetInsurancePurchaseListResponse>(sql)).ToList();

            return Result<List<GetInsurancePurchaseListResponse>>.Success(result);
        }
    }
}
