using Modules.Shared.Domain;

namespace HR.Domain.Leaves
{
    public static class LeaveErrors
    {
        public static readonly Error EmployeeRequired =
            new("Leave.EmployeeRequired", "يجب اختيار موظف");

        public static readonly Error StartDateRequired =
            new("Leave.StartDateRequired", "تاريخ البداية مطلوب");

        public static readonly Error EndDateRequired =
            new("Leave.EndDateRequired", "تاريخ النهاية مطلوب");

        public static readonly Error InvalidDateRange =
            new("Leave.InvalidDateRange", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");

        public static readonly Error CategoryRequired =
            new("Leave.CategoryRequired", "يجب تحديد نوع الأجازة");

        public static readonly Error NotPending =
            new("Leave.NotPending", "لا يمكن معالجة طلب ليس قيد المراجعة");

        public static readonly Error NotFound =
            new("Leave.NotFound", "طلب الأجازة غير موجود");

        public static readonly Error InsufficientBalance =
            new("Leave.InufficientBalance", "الرصيد غير كافٍ");

        public static readonly Error HajjServiceRequirement =
            new("Leave.HajjServiceRequirement", "يجب قضاء 5 سنوات خدمة على الأقل لأجازة الحج");

        public static readonly Error HajjAlreadyTaken =
            new("Leave.HajjAlreadyTaken", "تم استحقاق أجازة الحج من قبل");

        public static readonly Error MaternityLimitExceeded =
            new("Leave.MaternityLimitExceeded", "تم استنفاد الحد الأقصى لأجازات الوضع (3 مرات)");
    }
}
