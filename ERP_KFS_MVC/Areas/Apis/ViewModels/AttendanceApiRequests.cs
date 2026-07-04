using HR.Application.Attendance.Commands.CreateManualAttendance;
using HR.Domain.Permissions;

namespace ERP_KFS_MVC.Areas.Apis.ViewModels
{
    public class CheckInOutRequest
    {
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateManualAttendanceRequest
    {
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public MovementType MovementType { get; set; }
        public TimeSpan Time { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateAttendanceRequest
    {
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string? Notes { get; set; }
    }

    public class ConvertToPermissionRequest
    {
        public PermissionType PermissionType { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string? Notes { get; set; }
    }

    public class ConvertToVacationRequest
    {
        public string VacationType { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
