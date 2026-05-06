using Dapper;
using HR.Application.Payrolls.CalculatePayrollCycle;
using HR.Domain.Employees;
using HR.Domain.Payrolls;
using Modules.Shared.Application.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Services
{
    /// <summary>
    /// Implementation — بتجمع:
    /// الراتب الأساسي + الحوافز + التأمينات + قسط السلفة + شراء المدد + الجزاءات
    /// </summary>
    internal sealed class PayrollCalculationService : IPayrollCalculationService
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public PayrollCalculationService(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<PayrollEntry>> CalculateAsync(int month, int year, Guid? employmentTypeId, Guid cycleId, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            // جلب كل الموظفين مع بياناتهم المالية دفعة واحدة
            var sql = """
                SELECT
                    e.[Id]                                          AS EmployeeId,
                    ISNULL(ef.[BasicSalary2019], 0)                AS BasicSalary,
                    ISNULL(ef.[Incentives], 0)                     AS Incentives,
                    ISNULL(ef.[GrossSalary], 0) 
                        - ISNULL(ef.[BasicSalary2019], 0)          AS Allowances,

                    -- (1) أقساط السلف النشطة
                    COALESCE((
                        SELECT SUM(l.[InstallmentAmount])
                        FROM [HR].[Loans] l
                        WHERE l.[EmployeeId] = e.[Id]
                          AND l.[IsCompleted] = 0
                          AND l.[StartDate] <= DATEFROMPARTS(@Year, @Month, 1)
                    ), 0) AS LoanDeduction,

                    -- (2) أقساط شراء المدد التأمينية المعتمدة
                    COALESCE((
                        SELECT SUM(ip.[MonthlyInstallment])
                        FROM [HR].[InsurancePeriodPurchases] ip
                        WHERE ip.[EmployeeId] = e.[Id]
                          AND ip.[Status] = 'Approved'
                          AND ip.[DeductionStartDate] <= DATEFROMPARTS(@Year, @Month, 1)
                    ), 0) AS InsurancePurchaseDeduction,

                    -- (3) جزاءات هذا الشهر
                    COALESCE((
                        SELECT SUM(pr.[DeductionDays]) * (ISNULL(ef.[BasicSalary2019], 0) / 30.0)
                        FROM [HR].[PenaltyRecords] pr
                        WHERE pr.[EmployeeId] = e.[Id]
                          AND pr.[ActionType] IN (2, 3)
                          AND MONTH(pr.[ExecutionMonth]) = @Month
                          AND YEAR(pr.[ExecutionMonth])  = @Year
                    ), 0) AS PenaltyDeduction

                FROM [HR].[Employees] e
                LEFT JOIN [HR].[EmployeeFinancials] ef ON ef.[EmployeeId] = e.[Id]
                WHERE e.[IsActive] = 1
                ORDER BY e.[Name];
                """;

            var rows = await connection.QueryAsync(sql, new
            {
                Month = month,
                Year = year,
                EmploymentTypeId = employmentTypeId
            });

            var entries = new List<PayrollEntry>();

            foreach (var row in rows)
            {
                decimal basicSalary = (decimal)row.BasicSalary;
                decimal allowances = (decimal)row.Allowances;

                // لو مفيش بيانات مالية، تجاهل الموظف
                if (basicSalary == 0) continue;

                decimal insurance = Math.Round(basicSalary * 0.11m, 2);
                decimal tax = CalculateTax(basicSalary);

                var entry = PayrollEntry.Create(
                    cycleId: cycleId,
                    employeeId: (Guid)row.EmployeeId,
                    basicSalary: basicSalary,
                    incentives: (decimal)row.Incentives,
                    allowances: allowances,
                    insuranceDeduction: insurance,
                    taxDeduction: tax,
                    loanDeduction: (decimal)row.LoanDeduction,
                    insurancePurchaseDeduction: (decimal)row.InsurancePurchaseDeduction,
                    penaltyDeduction: (decimal)row.PenaltyDeduction);

                entries.Add(entry.Value!);
            }

            return entries;
        }

        /// <summary>حساب ضريبة الدخل المصرية — شرائح مبسطة</summary>
        private static decimal CalculateTax(decimal basicSalary)
        {
            var annual = basicSalary * 12;
            return annual switch
            {
                <= 15000 => 0,
                <= 30000 => Math.Round(basicSalary * 0.025m, 2), // ضريبة بنسبة 2.5%
                <= 45000 => Math.Round(basicSalary * 0.10m, 2), // نسبة الضريبة هي 10%.
                <= 60000 => Math.Round(basicSalary * 0.15m, 2), // نسبة الضريبة هي 15%.
                <= 200000 => Math.Round(basicSalary * 0.20m, 2), // نسبة الضريبة هي 20%.
                _ => Math.Round(basicSalary * 0.25m, 2) // نسبة الضريبة هي 25%.
            };
        }
    }
}
