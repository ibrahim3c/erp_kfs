using Modules.Shared.Domain;

namespace HR.Domain.Decisions
{
    public static class DecisionErrors
    {
        public static readonly Error EmployeeIdEmpty = new Error(
            "Employee.EmployeeIdEmpty",
            "معرف الموظف مطلوب لربط فرد العائلة به");

        public static readonly Error DecisionIdEmpty = new Error(
    "EmployeeDecision.DecisionIdEmpty",
    "معرف القرار مطلوب");

        public static readonly Error InvalidDecisionDates = new Error(
            "EmployeeDecision.InvalidDecisionDates",
            "تاريخ نهاية القرار يجب أن يكون بعد تاريخ البداية");
        public static readonly Error InvalidEndDate = new Error(
            "EmployeeDecision.InvalidEndDate",
            "تاريخ إنهاء القرار غير صالح");
        public static readonly Error DecisionAlreadyCancelled = new Error(
            "EmployeeDecision.DecisionAlreadyCancelled",
            "القرار ملغي بالفعل");
        public static readonly Error DecisionAlreadyEnded = new Error(
            "EmployeeDecision.DecisionAlreadyEnded",
            "القرار منتهي بالفعل");


        public static readonly Error NumberEmpty =
            new("Decision.NumberEmpty", "رقم القرار مطلوب");
        public static readonly Error CodeEmpty =
            new("Decision.CodeEmpty ", "رقم القرار مطلوب");

        public static readonly Error DecisionTypeEmpty =
            new("Decision.TypeEmpty", "نوع القرار مطلوب");
        public static readonly Error DecisionAuthorityEmpty =
            new("Decision.AuthorityEmpty", "جهة إصدار القرار مطلوبة");
        public static readonly Error InvalidDates =
            new("Decision.InvalidDates", "تواريخ القرار غير صحيحة");
        public static readonly Error AlreadyProcessed =
            new("Decision.AlreadyProcessed", "تم معالجة القرار مسبقاً");
        public static readonly Error AlreadyArchived =
            new("Decision.AlreadyArchived", "القرار مؤرشف بالفعل");


        public static readonly Error AuthorityNameEmpty =
             new("DecisionAuthority.NameEmpty", "اسم جهة القرار مطلوب");
        public static readonly Error AlreadyActive =
            new("DecisionAuthority.AlreadyActive", "الجهة نشطة بالفعل");
        public static readonly Error AlreadyInactive =
            new("DecisionAuthority.AlreadyInactive", "الجهة غير نشطة بالفعل");

        public static readonly Error NameEmpty =
             new("DecisionType.NameEmpty", "اسم نوع القرار مطلوب");


    }
}

