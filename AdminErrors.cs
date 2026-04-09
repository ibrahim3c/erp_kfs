using System;

using Modules.Shared.Domain;

namespace HR.Domain.Admin
{
    public static class AdminErrors
    {
        public static readonly Error CodeEmpty =
            new("AcademicIncentiveType.CodeEmpty", "كود الحافز العلمي مطلوب");

        public static readonly Error NameEmpty =
            new("AcademicIncentiveType.NameEmpty", "اسم الحافز العلمي مطلوب");

        public static readonly Error InvalidValue =
            new("AcademicIncentiveType.InvalidValue", "قيمة الحافز يجب أن تكون أكبر من صفر");

        public static readonly Error InvalidValueType =
            new("AcademicIncentiveType.InvalidValueType", "يجب تحديد نوع القيمة (نسبة أو مبلغ)");

        public static readonly Error AlreadyInactive =
            new("AcademicIncentiveType.AlreadyInactive", "الحافز غير نشط بالفعل");

        public static readonly Error AlreadyActive =
            new("AcademicIncentiveType.AlreadyActive", "الحافز نشط بالفعل");
    }
}
