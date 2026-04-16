using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace HR.Application.Payrolls.GetPayrollCycle
{
    public sealed class GetPayrollCycleQueryHandler
        : IQueryHandler<GetPayrollCycleQuery, GetPayrollCycleResponse?>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetPayrollCycleQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetPayrollCycleResponse?>> Handle(
            GetPayrollCycleQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            // جلب الدورة
            var cycleSql = """
                SELECT
                    pc."Id"     AS CycleId,
                    pc."Month"  AS Month,
                    pc."Year"   AS Year,
                    pc."Status" AS Status
                FROM "HR"."PayrollCycles" pc
                WHERE pc."Month" = @Month AND pc."Year" = @Year
                ORDER BY pc."CreatedAt" DESC
                LIMIT 1;
                """;

            var cycle = await connection.QueryFirstOrDefaultAsync<dynamic>(
                cycleSql,
                new { request.Month, request.Year }
                );

            if (cycle is null)
                return Result<GetPayrollCycleResponse?>.Success(null);

           // جلب مفردات الموظفين(مع حساب التسويات اليدوية)
            var entriesSql = """
                SELECT 
                    pe."Id"                         AS EntryId,
                    e."Name"                        AS EmployeeName,
                    
                    pe."BasicSalary"                AS BasicSalary,
                    pe."Incentives"                 AS Incentives,
                    pe."Allowances"                 AS Allowances,
                    
                    -- جلب التسويات اليدوية (لو مفيش هترجع صفر
                    COALESCE((
                    SELECT SUM("Amount") FROM "HR"."PayrollAdjustments" 
                    WHERE "EntryId" = pe."Id" AND "Type" = 'Addition'),
                    0) AS ManualAdditions,

                    COALESCE((
                    SELECT SUM("Amount") FROM "HR"."PayrollAdjustments" 
                    WHERE "EntryId" = pe."Id" AND "Type" = 'Deduction'), 
                    0) AS ManualDeductions,

                    -- إجمالي الاستحقاقات (الأساسي + مكمل + بدلات + مكافآت يدوية
                    pe."BasicSalary" 
                        + pe."Incentives" 
                        + pe."Allowances" 
                        + COALESCE((
                            SELECT SUM("Amount") FROM "HR"."PayrollAdjustments" 
                            WHERE "EntryId" = pe."Id" AND "Type" = 'Addition'),
                            0) AS GrossSalary,
                    
                    -- إجمالي الاستقطاعات
                    pe."InsuranceDeduction" 
                        + pe."TaxDeduction" 
                        + pe."LoanDeduction" 
                        + pe."InsurancePurchaseDeduction" 
                        + pe."PenaltyDeduction" 
                        + COALESCE((
                            SELECT SUM("Amount") FROM "HR"."PayrollAdjustments" 
                            WHERE "EntryId" = pe."Id" AND "Type" = 'Deduction'), 
                            0) AS TotalDeductions,
                    
                    -- الصافي (الاستحقاقات - الاستقطاعات
                    (pe."BasicSalary" 
                        + pe."Incentives" 
                        + pe."Allowances" 
                        + COALESCE((
                            SELECT SUM("Amount") FROM "HR"."PayrollAdjustments"
                            WHERE "EntryId" = pe."Id" AND "Type" = 'Addition'),
                            0)) AS TotalEarnings,
                    - 
                    (pe."InsuranceDeduction" 
                        + pe."TaxDeduction" 
                        + pe."LoanDeduction" 
                        + pe."InsurancePurchaseDeduction" 
                        + pe."PenaltyDeduction" 
                        + COALESCE((
                            SELECT SUM("Amount") FROM "HR"."PayrollAdjustments" 
                            WHERE "EntryId" = pe."Id" AND "Type" = 'Deduction'), 
                            0)) AS NetSalary,

                    -- تفاصيل الخصومات (باستخدام CONCAT بدل || عشان تتوافق مع أي SQL Engine)
                    CASE
                        WHEN pe."PenaltyDeduction" > 0 THEN CONCAT('(شامل ', pe."PenaltyDeduction", ' جنيه جزاءات)')
                        ELSE '(تأمينات + ضرائب)'
                    END                             AS DeductionDetails

                FROM "HR"."PayrollEntries" pe
                INNER JOIN "HR"."Employees" e ON pe."EmployeeId" = e."Id"
                WHERE pe."CycleId" = @CycleId
                ORDER BY e."Name";
                """;

            var entries = (await connection.QueryAsync<PayrollEntryDto>(
                entriesSql, 
                new { CycleId = (Guid)cycle.CycleId }
                )).ToList();

            var response = new GetPayrollCycleResponse
            {
                CycleId = (Guid)cycle.CycleId,
                Month = (int)cycle.Month,
                Year = (int)cycle.Year,
                Status = (string)cycle.Status,
                EmployeeCount = entries.Count,
                TotalDeductions = entries.Sum(e => e.TotalDeductions),
                TotalNetSalary = entries.Sum(e => e.NetSalary),
                Entries = entries
            };

            return Result<GetPayrollCycleResponse?>.Success(response);
        }
    }
}
