using Modules.Shared.Domain;

namespace HR.Domain.Legal
{
    public static class RulingErrors
    {
        public static readonly Error CaseNumberRequired =
            new("Ruling.CaseNumberRequired", "رقم الدعوى مطلوب");

        public static readonly Error YearRequired =
            new("Ruling.YearRequired", "السنة القضائية مطلوبة");

        public static readonly Error EmployeeRequired =
            new("Ruling.EmployeeRequired", "يجب اختيار الموظف المدعي");

        public static readonly Error SummaryRequired =
            new("Ruling.SummaryRequired", "منطوق الحكم مطلوب");

        public static readonly Error ExecutionTypeRequired =
            new("Ruling.ExecutionTypeRequired", "نوع التنفيذ مطلوب");

        public static readonly Error AlreadyExecuted =
            new("Ruling.AlreadyExecuted", "هذا الحكم تم تنفيذه بالفعل");

        public static readonly Error NotExecuted =
            new("Ruling.NotExecuted", "لا يمكن أرشفة حكم لم ينفذ بعد");

        public static readonly Error NotFound =
            new("Ruling.NotFound", "الحكم القضائي غير موجود");
    }
}
