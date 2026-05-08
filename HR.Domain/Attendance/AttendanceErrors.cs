using Modules.Shared.Domain;

namespace HR.Domain.Attendance
{
    public static class AttendanceErrors
    {
        public static readonly Error EmployeeRequired =
            new("Attendance.EmployeeRequired", "يجب اختيار الموظف");

        public static readonly Error DateRequired =
            new("Attendance.DateRequired", "التاريخ مطلوب");

        public static readonly Error FutureDateNotAllowed =
            new("Attendance.FutureDateNotAllowed", "لا يمكن تسجيل حضور ليوم مستقبلي");

        public static readonly Error CheckOutBeforeCheckIn =
            new("Attendance.CheckOutBeforeCheckIn", "وقت الخروج يجب أن يكون بعد وقت الدخول");

        public static readonly Error DuplicateRecord =
            new("Attendance.DuplicateRecord", "تم تسجيل هذا الموظف مسبقاً في هذا اليوم");

        public static readonly Error NotFound =
            new("Attendance.NotFound", "سجل الحضور غير موجود");

        public static readonly Error AlreadyCheckedOut =
            new("Attendance.AlreadyCheckedOut", "تم تسجيل خروج هذا الموظف مسبقاً");

        public static readonly Error MissingCheckIn =
            new("Attendance.MissingCheckIn", "يجب تسجيل الدخول أولاً قبل تسجيل الخروج");
    }
}
