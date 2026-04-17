using Modules.Shared.Domain;

namespace HR.Domain.JobStructures
{
    public static class JobStructureErrors
    {
        public static readonly Error CodeEmpty =
            new("JobStructures.CodeEmpty", "لا يمكن أن يكون الكود فارغاً.");

        public static readonly Error NameEmpty =
            new("JobStructures.NameEmpty", "لا يمكن أن يكون الاسم فارغاً.");

        public static readonly Error QualitativeGroupIdEmpty =
            new("JobStructures.QualitativeGroupIdEmpty", "معرف المجموعة النوعية مطلوب.");

        public static readonly Error FunctionalGroupIdEmpty =
            new("JobStructures.FunctionalGroupIdEmpty", "معرف المجموعة الوظيفية مطلوب.");

        public static readonly Error InvalidGradeLevel =
            new("JobStructures.InvalidGradeLevel", "يجب أن يكون مستوى الدرجة أكبر من الصفر.");

        public static readonly Error InvalidYearsNo =
            new("JobStructures.InvalidYearsNo", "لا يمكن أن يكون عدد السنوات رقماً سالباً.");

        public static readonly Error AlreadyActive =
            new("JobStructures.AlreadyActive", "هذا السجل مفعل بالفعل.");

        public static readonly Error AlreadyInactive =
            new("JobStructures.AlreadyInactive", "هذا السجل معطل بالفعل.");

        public static readonly Error NotFound =
            new("JobStructures.NotFound", "السجل غير موجود.");
    }
}

