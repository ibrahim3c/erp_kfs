using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Promotions.Entities
{
    /// <summary>
    /// تقرير الكفاءة السنوي للموظف
    /// </summary>
    public class KpiReport : Entity
    {
        public Guid EmployeeId { get; private set; }
        public int Year { get; private set; }

        // النسبة المئوية 0-100
        public decimal Score { get; private set; }

        // ممتاز / كفء / فوق المتوسط / متوسط / دون المتوسط
        public string Grade { get; private set; } = string.Empty;

        public decimal EfficiencyScore { get; private set; }
        public decimal DisciplineScore { get; private set; }
        public decimal AchievementScore { get; private set; }

        public Guid? EvaluatorId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Notes { get; private set; }

        // Navigation
        public Employee Employee { get; private set; } = null!;
        public Employee? Evaluator { get; private set; }

        private KpiReport(
            Guid id, Guid employeeId, int year, decimal score, string grade,
            decimal efficiencyScore, decimal disciplineScore, decimal achievementScore,
            Guid? evaluatorId, string status, string? notes) : base(id)
        {
            EmployeeId = employeeId;
            Year = year;
            Score = score;
            Grade = grade;
            EfficiencyScore = efficiencyScore;
            DisciplineScore = disciplineScore;
            AchievementScore = achievementScore;
            EvaluatorId = evaluatorId;
            Status = status;
            Notes = notes;
        }

        public static Result<KpiReport> Create(
            Guid employeeId, int year, decimal score,
            decimal efficiencyScore, decimal disciplineScore, decimal achievementScore,
            Guid? evaluatorId = null, string status = "Approved", string? notes = null)
        {
            if (score is < 0 or > 100)
                return Result<KpiReport>.Failure(PromotionErrors.OutRangeOfScore);

            if (efficiencyScore is < 0 or > 30)
                return Result<KpiReport>.Failure(PromotionErrors.InvalidSubScore);

            if (disciplineScore is < 0 or > 30)
                return Result<KpiReport>.Failure(PromotionErrors.InvalidSubScore);

            if (achievementScore is < 0 or > 40)
                return Result<KpiReport>.Failure(PromotionErrors.InvalidSubScore);

            var report = new KpiReport(
                Guid.NewGuid(), employeeId, year, score, CalculateGrade(score),
                efficiencyScore, disciplineScore, achievementScore,
                evaluatorId, status, notes);

            return Result<KpiReport>.Success(report);
        }

        private static string CalculateGrade(decimal score) => score switch
        {
            >= 90 => "\u0645\u0645\u062a\u0627\u0632",
            >= 80 => "\u0643\u0641\u0624",
            >= 70 => "\u0641\u0648\u0642 \u0627\u0644\u0645\u062a\u0648\u0633\u0637",
            >= 60 => "\u0645\u062a\u0648\u0633\u0637",
            _ => "\u062f\u0648\u0646 \u0627\u0644\u0645\u062a\u0648\u0633\u0637"
        };
    }
}
