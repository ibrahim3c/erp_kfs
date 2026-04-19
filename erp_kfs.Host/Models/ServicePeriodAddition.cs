// Models/ServicePeriodAddition.cs
using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class ServicePeriodAddition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EmployeeId { get; set; } = string.Empty;
        public virtual EmployeeAdmin Employee { get; set; } = null!;

        [Display(Name = "نوع المدة")]
        [Required]
        public ServicePeriodType PeriodType { get; set; }

        [Display(Name = "تاريخ البدء")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        [Required]
        public DateTime EndDate { get; set; }

        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
        // ✅ قابلة للتعديل (ليست للقراءة فقط)
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

        [Display(Name = "المستند")]
        [Required]
        public string DocumentPath { get; set; } = string.Empty;

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "تاريخ التقديم")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "سبب الرفض")]
        public string? RejectionReason { get; set; }
    }

    public enum ServicePeriodType
    {
        [Display(Name = "الخدمة بالتعاقد")]
        Contract = 1,
        [Display(Name = "الإعارة")]
        Secondment = 2,
        [Display(Name = "الانتداب")]
        Delegation = 3,
        [Display(Name = "التعيين المؤقت")]
        TemporaryAppointment = 4,
        [Display(Name = "الخدمة بالاستئجار")]
        Lease = 5,
        [Display(Name = "الوظيفة المؤقتة")]
        TemporaryPosition = 6
    }
}