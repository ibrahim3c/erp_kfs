

namespace HR.Domain.Promotions.ValueObjects
{
    /// <summary>
    /// معايير البحث — Value Object يُمرَّر للـ Domain Service
    /// كل الشروط اللي بيحددها HR قبل تشغيل البحث
    /// </summary>
    public sealed class EligibilityCriteria
    {
        public int MinKpiScore { get; }
        public int MaxPenaltyDays { get; }
        public DateTime EligibilityDate { get; }
        public int KpiYearsToCheck { get; }


        private EligibilityCriteria(
            int minKpiScore, int maxPenaltyDays,
            DateTime eligibilityDate, int kpiYearsToCheck)
        {
            MinKpiScore = minKpiScore;
            MaxPenaltyDays = maxPenaltyDays;
            EligibilityDate = eligibilityDate;
            KpiYearsToCheck = kpiYearsToCheck;
        }

        public static EligibilityCriteria ForPromotion(DateTime date)
            => new(minKpiScore: 70, maxPenaltyDays: 10,
                   eligibilityDate: date, kpiYearsToCheck: 2);

        public static EligibilityCriteria ForPeriodic(DateTime date)
            => new(minKpiScore: 0, maxPenaltyDays: 999,
                   eligibilityDate: date, kpiYearsToCheck: 1);

        public static EligibilityCriteria ForIncentive(DateTime date)
            => new(minKpiScore: 90, maxPenaltyDays: 0,
                   eligibilityDate: date, kpiYearsToCheck: 2);

        public static EligibilityCriteria Custom(
            int minKpiScore, int maxPenaltyDays,
            DateTime date, int kpiYearsToCheck = 2)
            => new(minKpiScore, maxPenaltyDays, date, kpiYearsToCheck);
    }
}
