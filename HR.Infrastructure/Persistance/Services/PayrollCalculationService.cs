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

namespace HR.Infrastructure.Persistance.Services
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

        public async Task<List<PayrollEntry>> CalculateAsync(int month, int year, EmploymentType employeeCategory, Guid cycleId, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            // جلب كل الموظفين مع بياناتهم المالية دفعة واحدة
            var sql = """
                SELECT
                    e."Id"           AS EmployeeId,
                    e."BasicSalary"  AS BasicSalary,
                    e."Incentives"   AS Incentives,
                    e."Allowances"   AS Allowances,

                    -- (1) أقساط السلف النشطة
                    COALESCE((
                        SELECT SUM(l."InstallmentAmount")
                        FROM "HR"."Loans" l
                        WHERE l."EmployeeId" = e."Id"
                          AND l."IsCompleted" = false
                          AND l."StartDate" <= MAKE_DATE(@Year, @Month, 1)
                    ), 0) AS LoanDeduction,

                    -- (2) أقساط شراء المدد التأمينية المعتمدة
                    COALESCE((
                        SELECT SUM(ip."MonthlyInstallment")
                        FROM "HR"."InsurancePeriodPurchases" ip
                        WHERE ip."EmployeeId" = e."Id"
                          AND ip."Status" = 'Approved'
                          AND ip."DeductionStartDate" <= MAKE_DATE(@Year, @Month, 1)
                    ), 0) AS InsurancePurchaseDeduction,

                  -- (3) جزاءات هذا الشهر
                    COALESCE((
                        SELECT SUM(pr."DeductionDays") * (e."BasicSalary" / 30.0)
                        FROM "HR"."PenaltyRecords" pr
                        WHERE pr."EmployeeId" = e."Id"
                          AND pr."ActionType" IN (2, 3)     -- ( 2:الوقف (الخصم 3:)(العقوبات المالية فقط
                          AND EXTRACT(MONTH FROM pr."ExecutionMonth") = @Month
                          AND EXTRACT(YEAR  FROM pr."ExecutionMonth") = @Year
                    ), 0)                       AS PenaltyDeduction

                FROM "HR"."Employees" e
                WHERE e."IsActive" = true
                ORDER BY e."Name";
                """;

            var rows = await connection.QueryAsync(sql, new
            {
                Month = month,
                Year = year,
                EmployeeCategory = employeeCategory
            });

            var entries = new List<PayrollEntry>();

            foreach (var row in rows)
            {
                decimal basicSalary = (decimal)row.BasicSalary;
                decimal insurance = Math.Round(basicSalary * 0.11m, 2); // 11% تأمينات
                decimal tax = CalculateTax(basicSalary);           // شريحة ضريبية

                var entry = PayrollEntry.Create(
                    cycleId: cycleId,
                    employeeId: (Guid)row.EmployeeId,
                    basicSalary: basicSalary,
                    incentives: (decimal)row.Incentives,
                    allowances: (decimal)row.Allowances,
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
