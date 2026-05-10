using HR.Domain.Employees;
using HR.Domain.Promotions.Enum;
using HR.Domain.Promotions.Snapshots;
using HR.Domain.Promotions.ValueObjects;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Entities
{

    /// <summary>
    /// نتيجة فحص موظف في دورة معينة
    /// هي اللي بتملأ صفوف الجدول في الصفحة
    /// </summary>
    public class EligibilityResult : Entity
    {
        public Guid PromotionCycleId { get; private set; }
        public Guid EmployeeId { get; private set; }
        public EligibilityStatus Status { get; private set; }
        public ExclusionReason ExclusionReason { get; private set; }

        public Guid CurrentGradeId { get; private set; }
        public string CurrentGradeCode { get; private set; } = string.Empty;
        public string CurrentGradeName { get; private set; } = string.Empty;
        public int CurrentGradeLevel { get; private set; }

        public int? ProposedGradeLevel { get; private set; }

        public decimal AvgKpiScore { get; private set; }
        public decimal? PenaltyDays { get; private set; }
        public decimal YearsInCurrentGrade { get; private set; }
        public bool IsSelected { get; private set; }

        // Navigation
        public PromotionCycle Cycle { get; private set; } = null!;

        private EligibilityResult() : base(Guid.NewGuid()) { } // EF only

        //  Constructor الحقيقي — بيعين كل الـ properties
        private EligibilityResult(
            Guid cycleId,
            Guid employeeId,
            EligibilityStatus status,
            ExclusionReason exclusionReason,
            Guid currentGradeId,
            string currentGradeCode,
            string currentGradeName,
            int currentGradeLevel,
            int? proposedGradeLevel,
            decimal avgKpiScore,
            decimal? penaltyDays,
            decimal yearsInCurrentGrade) : base(Guid.NewGuid())
        {
            PromotionCycleId = cycleId;
            EmployeeId = employeeId;
            Status = status;
            ExclusionReason = exclusionReason;
            CurrentGradeId = currentGradeId;
            CurrentGradeCode = currentGradeCode;
            CurrentGradeName = currentGradeName;
            CurrentGradeLevel = currentGradeLevel;
            ProposedGradeLevel = proposedGradeLevel;
            AvgKpiScore = avgKpiScore;
            PenaltyDays = penaltyDays;
            YearsInCurrentGrade = yearsInCurrentGrade;
            IsSelected = false;
        }

        //  Factory Methods 
        public static Result<EligibilityResult> CreateEligible(
          Guid cycleId,
          Guid employeeId,
          JobGradeSnapshot currentGrade,    // ← Snapshot مش JobGrade VO
          int? proposedGradeLevel,
          decimal avgKpi,
          decimal? penaltyDays,
          decimal yearsInGrade)
        {
            var result = new EligibilityResult(
                cycleId, employeeId,
                EligibilityStatus.Eligible,
                ExclusionReason.None,
                currentGrade.Id,       // GUID من Organization
                currentGrade.Code,
                currentGrade.Name,
                currentGrade.GradeLevel,
                proposedGradeLevel,
                avgKpi, penaltyDays, yearsInGrade);

            return Result<EligibilityResult>.Success(result);
        }

        public static Result<EligibilityResult> CreateExcluded(
            Guid cycleId,
            Guid employeeId,
            JobGradeSnapshot currentGrade,    // ← Snapshot
            ExclusionReason reason,
            decimal avgKpi,
            decimal? penaltyDays,
            decimal yearsInGrade)
        {
            var result = new EligibilityResult(
                cycleId, employeeId,
                EligibilityStatus.Excluded,
                reason,
                currentGrade.Id,
                currentGrade.Code,
                currentGrade.Name,
                currentGrade.GradeLevel,
                null,
                avgKpi, penaltyDays, yearsInGrade);

            return Result<EligibilityResult>.Success(result);
        }
        public Result SetSelected(bool selected)
        {
            if (Status == EligibilityStatus.Excluded && selected)
                return Result.Failure(PromotionErrors.IsExclused);

            IsSelected = selected;
            return Result.Success();
        }
    }
}

        