using Dapper;
using HR.Application.Employees.GetAllEmployees;
using HR.Application.Promotions.DTOs;
using HR.Domain;
using HR.Domain.Employees;
using HR.Domain.Penalties;
using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Enum;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Promotions.Services;
using HR.Domain.Promotions.Snapshots;
using HR.Domain.Promotions.ValueObjects;
using MediatR;
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

    public class CheckEligibilityHandler
        : IQueryHandler<CheckEligibilityQuery, CheckEligibilityResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IHRUnitOfWork _uow;
        private readonly EligibilityEngine _engine;

        public CheckEligibilityHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IHRUnitOfWork uow,
            EligibilityEngine engine)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _uow = uow;
            _engine = engine;
        }

        public async Task<Result<CheckEligibilityResponse>> Handle(
            CheckEligibilityQuery request,
            CancellationToken cancellationToken)
        {
            // ── 1. بناء المعايير ─────────────────────────────────────
            var criteria = request.CycleType switch
            {
                CycleType.Promotion => EligibilityCriteria.ForPromotion(request.EligibilityDate),
                CycleType.Periodic => EligibilityCriteria.ForPeriodic(request.EligibilityDate),
                CycleType.Incentive => EligibilityCriteria.ForIncentive(request.EligibilityDate),
                _ => EligibilityCriteria.Custom(
                                           request.MinKpiScore,
                                           request.MaxPenaltyDays,
                                           request.EligibilityDate)
            };

            // ── 2. إنشاء الكشف ───────────────────────────────────────
            var cycleResult = PromotionCycle.Create(
                request.CycleType,
                criteria,
                request.RequestByUserId);

            if (cycleResult.IsFailure)
                return Result<CheckEligibilityResponse>.Failure(cycleResult.Error);

            var cycle = cycleResult.Value;

            var cycleId = await _uow.PromotionCycleRepository
                                    .SaveCycleAsync(cycle, cancellationToken);

            // ── 3. جلب الموظفين بـ Dapper ────────────────────────────
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                e.Id                                        AS Id,
                e.Name                                      AS Name,
                ISNULL(ou.Name, N'—')                       AS Department,
                jg.Id                                       AS GradeId,
                jg.Code                                     AS GradeCode,
                jg.Name                                     AS GradeName,
                jg.GradeLevel                               AS GradeLevel,
                jg.YearsNo                                  AS GradeYearsNo,
                ISNULL(
                    CAST(
                        (SELECT MAX(ph.EffectiveDate)
                         FROM   HR.PromotionHistory ph
                         WHERE  ph.EmployeeId = e.Id) AS date),
                    CAST(e.HireDate AS date)
                )                                           AS GradeStartDate
            FROM      HR.Employees           e
            LEFT JOIN Organization.JobGrades jg ON jg.Id = e.JobGradeId
            LEFT JOIN Organization.OrgUnits  ou ON ou.Id = e.OrgUnitId
            WHERE     e.IsActive     = 1
            AND       e.JobGradeId   IS NOT NULL
            ORDER BY  jg.GradeLevel  ASC, e.Name ASC
            """;

            var employees = (await connection
                .QueryAsync<EmployeeSnapshot>(sql))
                .ToList();

            if (!employees.Any())
                return Result<CheckEligibilityResponse>.Success(
                    EmptyResponse(cycleId));

            // ── 4. حصة العلاوة التشجيعية — 10% من كل درجة ───────────
            // ✅ emp.GradeLevel موجود على الـ Snapshot مباشرة
            var quotaTracker = employees
                .GroupBy(e => e.GradeLevel)
                .ToDictionary(
                    g => g.Key,
                    g => (int)Math.Floor(g.Count() * 0.10m));

            var results = new List<EligibilityResultDto>();
            var failures = new List<string>();

            // ── 5. تقييم كل موظف ─────────────────────────────────────
            var allResults = new List<EligibilityResult>(); // ✅ قائمة منفصلة

            foreach (var emp in employees)
            {
                var kpiReports = await _uow.KpiReportRepository
                    .GetByEmployeeAsync(emp.Id, criteria.KpiYearsToCheck, cancellationToken);

                var penaltyDays = await _uow.PenaltyRepository
                    .GetTotalDaysAsync(emp.Id,
                        criteria.EligibilityDate.AddYears(-2),
                        cancellationToken);

                int quotaLeft = quotaTracker.GetValueOrDefault(emp.GradeLevel, 0);

                var evalResult = _engine.Evaluate(
                    cycle, emp, criteria, kpiReports, penaltyDays, quotaLeft);

                if (evalResult.IsFailure)
                {
                    failures.Add($"{emp.Name}: {evalResult.Error.Name}");
                    continue;
                }

                var eligResult = evalResult.Value;

                //  مش بنضيف للـ cycle — بنضيف للقائمة المنفصلة
                allResults.Add(eligResult);

                if (eligResult.Status == EligibilityStatus.Eligible
                    && request.CycleType == CycleType.Incentive)
                {
                    quotaTracker[emp.GradeLevel] = Math.Max(0, quotaLeft - 1);
                }

                results.Add(MapToDto(eligResult, emp));
            }

            // ── 6. حفظ النتائج ───────────────────────────────────────
            //  INSERT Results مباشرة — بدون Update على الـ cycle
            await _uow.PromotionCycleRepository
                      .AddResultsAsync(allResults, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            // ── 7. الرد ──────────────────────────────────────────────
            return Result<CheckEligibilityResponse>.Success(
                new CheckEligibilityResponse
                {
                    CycleId = cycleId,
                    TotalChecked = results.Count,
                    TotalEligible = results.Count(r => r.IsEligible),
                    TotalExcluded = results.Count(r => !r.IsEligible),
                    Failures = failures,
                    Items = results
                });
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static CheckEligibilityResponse EmptyResponse(Guid cycleId)
            => new()
            {
                CycleId = cycleId,
                TotalChecked = 0,
                TotalEligible = 0,
                TotalExcluded = 0,
                Items = new(),
                Failures = new()
            };

        private static EligibilityResultDto MapToDto(
            EligibilityResult result, EmployeeSnapshot emp)
            => new()
            {
                EmployeeId = result.EmployeeId,
                EmployeeName = emp.Name,
                Department = emp.Department,
                CurrentGrade = result.CurrentGradeName,
                GradeStartDate = emp.GradeStartDate,
                YearsInGrade = result.YearsInCurrentGrade,
                AvgKpiScore = result.AvgKpiScore,
                PenaltyDays = result.PenaltyDays,
                IsEligible = result.Status == EligibilityStatus.Eligible,
                ExclusionReason = GetExclusionText(result.ExclusionReason),
                ProposedAction = BuildProposedAction(result),
                IsSelected = result.IsSelected
            };

        private static string GetExclusionText(ExclusionReason reason)
            => reason switch
            {
                ExclusionReason.ExceededPenalties => "تجاوز الجزاءات",
                ExclusionReason.LowKpiScore => "ضعف تقرير الكفاءة",
                ExclusionReason.InsufficientYears => "لم يكمل المدة المطلوبة",
                ExclusionReason.AlreadyMaxGrade => "على أعلى درجة",
                ExclusionReason.IncentiveQuotaFull => "امتلأت حصة 10%",
                ExclusionReason.InvalidGradeChange => "خطأ في تحديد الدرجة",
                _ => string.Empty
            };

        private static string BuildProposedAction(EligibilityResult r)
        {
            if (r.Status == EligibilityStatus.Excluded) return "—";
            if (r.ProposedGradeLevel.HasValue)
                return $"ترقية للدرجة رقم {r.ProposedGradeLevel}";
            return "منح علاوة";
        }
    }
}
