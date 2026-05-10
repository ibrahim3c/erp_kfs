using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Navigation
        public Employee Employee { get; private set; } = null!;

        private KpiReport(Guid id,Guid employeeId,int year,decimal score,string grade) : base(id) {
            EmployeeId = employeeId;
            Year = year;
            Score = score;
            Grade = grade;
            
        } // EF

        public static Result<KpiReport> Create(Guid employeeId, int year, decimal score)
        {
            if (score is < 0 or > 100)
                return Result<KpiReport>.Failure(PromotionErrors.OutRangeOfScore);

            var report = new KpiReport(Guid.NewGuid(), employeeId, year, score, CalculateGrade(score));
            return Result<KpiReport>.Success(report);
      
        }

        private static string CalculateGrade(decimal score) => score switch
        {
            >= 90 => "ممتاز",
            >= 80 => "كفء",
            >= 70 => "فوق المتوسط",
            >= 60 => "متوسط",
            _ => "دون المتوسط"
        };
    }
}
