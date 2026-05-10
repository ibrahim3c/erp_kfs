using HR.Domain.Employees;
using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Enum;
using HR.Domain.Promotions.Snapshots;
using HR.Domain.Promotions.ValueObjects;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Services
{
    /// <summary>
    /// محرك الاستحقاق — Domain Service
    /// هو اللي بيفحص كل موظف ويقرر مستحق أو مستبعد وليه
    /// لا يعرف DB، بياخد البيانات جاهزة من الـ Application Layer
    /// </summary>
    public sealed class EligibilityEngine
    {
        public Result<EligibilityResult> Evaluate(
      PromotionCycle cycle,
      EmployeeSnapshot employee,
      EligibilityCriteria criteria,
      List<KpiReportDto> kpiReports,
      decimal? penaltyDays,
      int incentiveQuotaRemaining)
        {
            // ✅ بنبني Value Object من الـ Flat Snapshot
            var currentGrade = JobGrade.FromSnapshot(employee.ToGradeSnapshot());

            decimal avgKpi = kpiReports.Any()
                                    ? kpiReports.Average(k => k.Score) : 0;
            decimal yearsInGrade = CalculateYears(
                                    employee.GradeStartDate,
                                    criteria.EligibilityDate);

            if (!currentGrade.HasCompletedRequiredYears(yearsInGrade))
                return Excluded(cycle.Id, employee.Id,
                    employee.ToGradeSnapshot(),
                    ExclusionReason.InsufficientYears,
                    avgKpi, penaltyDays, yearsInGrade);

            if (penaltyDays > criteria.MaxPenaltyDays)
                return Excluded(cycle.Id, employee.Id,
                    employee.ToGradeSnapshot(),
                    ExclusionReason.ExceededPenalties,
                    avgKpi, penaltyDays, yearsInGrade);

            if (avgKpi < criteria.MinKpiScore)
                return Excluded(cycle.Id, employee.Id,
                    employee.ToGradeSnapshot(),
                    ExclusionReason.LowKpiScore,
                    avgKpi, penaltyDays, yearsInGrade);

            if (cycle.Type == CycleType.Incentive && incentiveQuotaRemaining <= 0)
                return Excluded(cycle.Id, employee.Id,
                    employee.ToGradeSnapshot(),
                    ExclusionReason.IncentiveQuotaFull,
                    avgKpi, penaltyDays, yearsInGrade);

            int? proposedGradeLevel = null;
            if (cycle.Type == CycleType.Promotion)
            {
                if (!currentGrade.HasNextGrade())
                    return Excluded(cycle.Id, employee.Id,
                        employee.ToGradeSnapshot(),
                        ExclusionReason.AlreadyMaxGrade,
                        avgKpi, penaltyDays, yearsInGrade);

                var nextResult = currentGrade.NextGradeLevel();
                if (nextResult.IsFailure)
                    return Result<EligibilityResult>.Failure(nextResult.Error);

                proposedGradeLevel = nextResult.Value;
            }

            return EligibilityResult.CreateEligible(
                cycle.Id, employee.Id,
                employee.ToGradeSnapshot(),
                proposedGradeLevel,
                avgKpi, penaltyDays, yearsInGrade);
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static Result<EligibilityResult> Excluded(
            Guid cycleId, Guid employeeId, JobGradeSnapshot grade,
            ExclusionReason reason, decimal avgKpi,
            decimal? penaltyDays, decimal yearsInGrade)
            => EligibilityResult.CreateExcluded(
                cycleId, employeeId, grade,
                reason, avgKpi, penaltyDays, yearsInGrade);

        private static decimal CalculateYears(DateTime start, DateTime reference)
        {
            var months = ((reference.Year - start.Year) * 12)
                       + reference.Month - start.Month;
            return Math.Round(months / 12m, 1);
        }
    }

    // DTO بسيط بيجي من الـ DB — ملوش هوية Domain
    
    public record KpiReportDto(int Year, decimal Score);
}
