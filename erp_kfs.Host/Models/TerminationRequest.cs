// Models/TerminationRequest.cs
using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class TerminationRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EmployeeId { get; set; } = string.Empty;
        public virtual EmployeeAdmin Employee { get; set; } = null!;

        [Display(Name = "سبب الإنهاء")]
        [Required]
        public string ReasonType { get; set; } = "Resignation"; // Resignation, Retirement, Death, Absence, Criminal, Disciplinary

        [Display(Name = "تاريخ الإنهاء")]
        [Required]
        public DateTime TerminationDate { get; set; }

        [Display(Name = "تفاصيل إضافية")]
        public string? Details { get; set; }

        [Display(Name = "المستند")]
        public string? DocumentPath { get; set; }

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Display(Name = "تاريخ الطلب")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←
        // ✅ الخاصية المطلوبة (كانت ناقصة)
        [Display(Name = "سبب الرفض")]
        public string? RejectionReason { get; set; }
        // ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ← ←

        public string? ServicePeriodAdditionId { get; set; }
        public virtual ServicePeriodAddition? ServicePeriodAddition { get; set; }
    }
}