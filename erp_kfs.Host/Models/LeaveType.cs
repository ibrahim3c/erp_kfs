using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyERP.Web.Models
{
    public class LeaveType
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "الاسم الداخلي مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العرض مطلوب")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "أقصى عدد أيام مطلوب")]
        public int MaxDays { get; set; }

        [Required]
        public bool RequiresApproval { get; set; } = true;

        // ← الخاصية الجديدة من طلباتك
        public string? AutoRenewDate { get; set; } // مثال: "01-01" (1 يناير كل سنة)

        public bool IsGenderSpecific { get; set; }

        [Required]
        public decimal SalaryPercentage { get; set; } = 100;

        public bool IsAnnualBasedOnService { get; set; }

        // ← الخاصية الأساسية من نظام الاعتماد المزدوج
        public bool IsCasual { get; set; } // true = إجازة عارضة (تعتمد فورًا)

        // العلاقات
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}