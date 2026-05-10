

namespace HR.Application.Promotions.DTOs
{
    public class CheckEligibilityResponse
    {
       
        public Guid CycleId { get; set; }
        public int TotalChecked { get; set; }
        public int TotalEligible { get; set; }
        public int TotalExcluded { get; set; }

        // موظفين فشل تقييمهم (خطأ في البيانات)
        public List<string> Failures { get; set; } = new();
        public List<EligibilityResultDto> Items { get; set; } = new();
    }

    public class EligibilityResultDto
    {
        public Guid EmployeeId { get; set; } 
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string CurrentGrade { get; set; } = string.Empty;
        public DateTime GradeStartDate { get; set; }
        public decimal YearsInGrade { get; set; }
        public decimal AvgKpiScore { get; set; }
        public decimal? PenaltyDays { get; set; }
        public bool IsEligible { get; set; }
        public string ExclusionReason { get; set; } = string.Empty;
        public string ProposedAction { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
