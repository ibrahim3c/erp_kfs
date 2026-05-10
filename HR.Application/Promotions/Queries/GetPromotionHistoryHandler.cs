using Dapper;
using HR.Application.Promotions.DTOs;
using HR.Domain.Promotions.Enum;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Promotions.Queries
{

    public class GetPromotionHistoryHandler
        : IQueryHandler<GetPromotionHistoryQuery, EmployeePromotionHistoryResponse>
    {
        private readonly ISqlConnectionFactory _sql;

        public GetPromotionHistoryHandler(ISqlConnectionFactory sql)
            => _sql = sql;

        public async Task<Result<EmployeePromotionHistoryResponse>> Handle(
            GetPromotionHistoryQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sql.CreateConnection();

            const string empSql = """
                SELECT
                    e.Id            AS Id,
                    e.Name          AS Name,
                    ISNULL(jg.Name, N'—') AS CurrentGrade
                FROM  HR.Employees           e
                LEFT JOIN Organization.JobGrades jg ON jg.Id = e.JobGradeId
                WHERE e.Id = @employeeId
                """;

            var emp = await connection.QuerySingleOrDefaultAsync<(Guid Id, string Name, string CurrentGrade)>(
                empSql, new { employeeId = request.EmployeeId });

            if (emp == default)
                return Result<EmployeePromotionHistoryResponse>.Failure(
                    new Error("EmployeeNotFound", "الموظف غير موجود"));

            // ── جلب سجل الحركات ─────────────────────────────────────
            const string historySql = """
                SELECT
                    ph.Id                               AS Id,
                    ph.EffectiveDate                    AS EffectiveDate,
                    ph.MovementType                     AS MovementType,
                    ISNULL(fg.Name, N'—')               AS FromGrade,
                    ISNULL(tg.Name, N'—')               AS ToGrade,
                    ph.Notes                            AS Notes,
                    ph.LinkedDecisionId                 AS LinkedDecisionId
                FROM  HR.PromotionHistory                ph
                LEFT JOIN Organization.JobGrades         fg ON fg.Id = ph.FromGradeId
                LEFT JOIN Organization.JobGrades         tg ON tg.Id = ph.ToGradeId
                WHERE ph.EmployeeId = @employeeId
                ORDER BY ph.EffectiveDate DESC
                """;

            var rows = await connection.QueryAsync<PromotionHistoryRow>(
                historySql, new { employeeId = request.EmployeeId });

            var items = rows.Select(r => new PromotionHistoryDto
            {
                Id = r.Id,
                EffectiveDate = r.EffectiveDate,
                MovementType = MapMovementType(r.MovementType),
                FromGrade = r.FromGrade,
                ToGrade = r.ToGrade,
                Notes = r.Notes,
                LinkedDecisionId = r.LinkedDecisionId
            }).ToList();

            return Result<EmployeePromotionHistoryResponse>.Success(
                new EmployeePromotionHistoryResponse
                {
                    EmployeeId = emp.Id,
                    EmployeeName = emp.Name,
                    CurrentGrade = emp.CurrentGrade,
                    Items = items
                });
        }

        private static string MapMovementType(int type) => type switch
        {
            (int)CycleType.Promotion => "ترقية",
            (int)CycleType.Periodic => "علاوة دورية",
            (int)CycleType.Incentive => "علاوة تشجيعية",
            _ => "حركة وظيفية"
        };

        // Dapper mapping class — خفيفة وداخلية
        private class PromotionHistoryRow
        {
            public Guid Id { get; set; }
            public DateTime EffectiveDate { get; set; }
            public int MovementType { get; set; }
            public string FromGrade { get; set; } = string.Empty;
            public string ToGrade { get; set; } = string.Empty;
            public string? Notes { get; set; }
            public Guid? LinkedDecisionId { get; set; }
        }
    }
}

