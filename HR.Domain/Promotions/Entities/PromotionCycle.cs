using HR.Domain.Promotions.Enum;
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
    /// دورة الحركة الوظيفية — Aggregate Root
    /// كل مرة HR يشغل بحث ترقيات → تنشأ PromotionCycle جديدة
    /// </summary>
    // HR.Domain/Promotions/Entities/PromotionCycle.cs
    public class PromotionCycle : Entity
    {
        public CycleType Type { get; private set; }
        public DateTime EligibilityDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        public bool IsApproved { get; private set; }
        public DateTime? ApprovedAt { get; private set; }
        public Guid? ApprovedByUserId { get; private set; }

        // المعايير — مخزنة مع الكشف للمرجعية
        public int MinKpiScore { get; private set; }
        public decimal? MaxPenaltyDays { get; private set; }
        public int KpiYearsToCheck { get; private set; }

        // Navigation
        private readonly List<EligibilityResult> _results = new();
        public IReadOnlyCollection<EligibilityResult> Results => _results.AsReadOnly();

        // EF Constructor — private بدون منطق
        private PromotionCycle() : base(Guid.NewGuid()) { }

        // Private Constructor الحقيقي — بيعين كل Properties
        private PromotionCycle(
            Guid id,
            CycleType type,
            DateTime eligibilityDate,
            DateTime createdAt,
            Guid createdByUserId,
            int minKpiScore,
            decimal? maxPenaltyDays,
            int kpiYearsToCheck) : base(id)
        {
            Type = type;
            EligibilityDate = eligibilityDate;
            CreatedAt = createdAt;
            CreatedByUserId = createdByUserId;
            IsApproved = false;
            MinKpiScore = minKpiScore;
            MaxPenaltyDays = maxPenaltyDays;
            KpiYearsToCheck = kpiYearsToCheck;
        }

        // YearsNo بقى في JobGrade نفسه — مش في الكشف
        public static Result<PromotionCycle> Create(
            CycleType type,
            EligibilityCriteria criteria,
            Guid createdByUserId)
        {
            if (createdByUserId == Guid.Empty)
                return Result<PromotionCycle>.Failure(
                    new Error("PromotionCycle.InvalidUser", "يجب تحديد المستخدم المنشئ"));

            var cycle = new PromotionCycle(
                Guid.NewGuid(),
                type,
                criteria.EligibilityDate,
                DateTime.UtcNow,
                createdByUserId,
                criteria.MinKpiScore,
                criteria.MaxPenaltyDays,
                criteria.KpiYearsToCheck);

            return Result<PromotionCycle>.Success(cycle);
        }

        // ✅ Approve — بترجع Result مش void
        public Result Approve(Guid approvedByUserId)
        {
            if (IsApproved)
                return Result.Failure(PromotionErrors.AlreadyIsApproved);

            if (!_results.Any(r => r.IsSelected))
                return Result.Failure(PromotionErrors.MustSelectOneAtLeast);

            IsApproved = true;
            ApprovedAt = DateTime.UtcNow;
            ApprovedByUserId = approvedByUserId;

            return Result.Success();
        }

        public void AddResult(EligibilityResult result)
            => _results.Add(result);
    }
}
