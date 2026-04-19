using System.ComponentModel.DataAnnotations;

namespace MyERP.Web.Models
{
    public class LeaveRequest
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string? EmployeeId { get; set; }
     public string? LeaveTypeId { get; set; }
       public DateTime? StartDate { get; set; }
           public DateTime RequestDate { get; set; } = DateTime.Now;
public DateTime? EndDate { get; set; }
        [Required, Range(0.5, 180)] public decimal DaysRequested { get; set; }
        [Required] public string Status { get; set; } = "PendingManager";
        public string? Notes { get; set; }
        public string? MedicalReportPath { get; set; }

        public virtual EmployeeAdmin? Employee { get; set; }
        public virtual LeaveType? LeaveType { get; set; }
    }
}