using Modules.Shared.Domain;


namespace HR.Domain.Permissions
{
    public static class AttendanceErrors
    {
        public static readonly Error EmployeeRequired =
            new("Attendance.EmployeeRequired", "يجب اختيار الموظف");

        public static readonly Error InvalidTimeRange =
            new("Attendance.InvalidTimeRange", "وقت العودة يجب أن يكون بعد وقت الخروج");

        public static readonly Error PersonalPermissionExceeded =
            new("Attendance.PersonalPermissionExceeded",
                "الإذن الشخصي لا يجوز أن يتجاوز 4 ساعات");

        public static readonly Error NotLate =
            new("Attendance.NotLate", "وقت الحضور قبل أو في موعد الدوام الرسمي");

        public static readonly Error NotFound =
            new("Attendance.NotFound", "السجل غير موجود");

        public static readonly Error MonthlyCountExceeded = 
            new("Attendance.MonthlyCountExceeded",
                "تجاوز الحد المسموح — مرتين شهرياً للتأخير");

        public static readonly Error MonthlyHoursExceeded =
            new("Attendance.MonthlyHoursExceeded",
                "تجاوز الحد المسموح — 4 ساعات شهرياً للإذن الشخصي");

    }
}
