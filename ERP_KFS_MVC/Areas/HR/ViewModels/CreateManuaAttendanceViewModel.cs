using HR.Application.Attendance.Commands.CreateManualAttendance;
using System.ComponentModel.DataAnnotations;

namespace ERP_KFS_MVC.Areas.HR.ViewModels
{
    public class CreateManualAttendanceViewModel
    {
        [Required(ErrorMessage = "يجب اختيار الموظف")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "التاريخ مطلوب")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "نوع الحركة مطلوب")]
        public MovementType MovementType { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
