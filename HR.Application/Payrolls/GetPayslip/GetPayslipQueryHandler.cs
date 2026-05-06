using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayslip
{
    public sealed class GetPayslipQueryHandler
     : IQueryHandler<GetPayslipQuery, PayslipResponse?>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetPayslipQueryHandler(ISqlConnectionFactory connectionFactory)
            => _connectionFactory = connectionFactory;

        public async Task<Result<PayslipResponse?>> Handle(
            GetPayslipQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = """
            SELECT
                e.[Name]                            AS EmployeeName,
                e.[Code]                            AS EmployeeCode,
                ISNULL(jt.[Name], N'—')             AS JobTitle,
                pc.[Month]                          AS Month,
                pc.[Year]                           AS Year,

                pe.[BasicSalary]                    AS BasicSalary,
                pe.[Incentives]                     AS Incentives,
                pe.[Allowances]                     AS Allowances,

                ISNULL((
                    SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                    WHERE [EntryId] = pe.[Id] AND [Type] = 'Addition'), 0)
                                                    AS ManualAdditions,

                pe.[BasicSalary] + pe.[Incentives] + pe.[Allowances]
                    + ISNULL((
                        SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                        WHERE [EntryId] = pe.[Id] AND [Type] = 'Addition'), 0)
                                                    AS GrossSalary,

                pe.[InsuranceDeduction]             AS InsuranceDeduction,
                pe.[TaxDeduction]                   AS TaxDeduction,
                pe.[LoanDeduction]                  AS LoanDeduction,
                pe.[InsurancePurchaseDeduction]     AS InsurancePurchaseDeduction,
                pe.[PenaltyDeduction]               AS PenaltyDeduction,

                ISNULL((
                    SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                    WHERE [EntryId] = pe.[Id] AND [Type] = 'Deduction'), 0)
                                                    AS ManualDeductions,

                pe.[InsuranceDeduction] + pe.[TaxDeduction] + pe.[LoanDeduction]
                    + pe.[InsurancePurchaseDeduction] + pe.[PenaltyDeduction]
                    + ISNULL((
                        SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                        WHERE [EntryId] = pe.[Id] AND [Type] = 'Deduction'), 0)
                                                    AS TotalDeductions,

                (pe.[BasicSalary] + pe.[Incentives] + pe.[Allowances]
                    + ISNULL((
                        SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                        WHERE [EntryId] = pe.[Id] AND [Type] = 'Addition'), 0))
                - (pe.[InsuranceDeduction] + pe.[TaxDeduction] + pe.[LoanDeduction]
                    + pe.[InsurancePurchaseDeduction] + pe.[PenaltyDeduction]
                    + ISNULL((
                        SELECT SUM([Amount]) FROM [HR].[PayrollAdjustments]
                        WHERE [EntryId] = pe.[Id] AND [Type] = 'Deduction'), 0))
                                                    AS NetSalary,

                ISNULL(ef.[BankName], N'—')         AS BankName,
                ISNULL(ef.[BankAccount], N'—')      AS BankAccount

            FROM [HR].[PayrollEntries] pe
            INNER JOIN [HR].[PayrollCycles]  pc ON pc.[Id]         = pe.[CycleId]
            INNER JOIN [HR].[Employees]      e  ON e.[Id]          = pe.[EmployeeId]
            LEFT JOIN [Organization].[JobTitles] jt ON jt.[Id] = e.[JobTitleId]
            LEFT  JOIN [HR].[EmployeeFinancials] ef ON ef.[EmployeeId] = e.[Id]
            WHERE pe.[Id] = @EntryId;
            """;

            var result = await connection.QueryFirstOrDefaultAsync<PayslipResponse>(
                sql, new { request.EntryId });

            return Result<PayslipResponse?>.Success(result);
        }
    }
}
