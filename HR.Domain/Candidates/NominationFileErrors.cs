using Modules.Shared.Domain;


namespace HR.Domain.Candidates
{
    public static class NominationFileErrors
    {
        public static readonly Error FilePathEmpty =
            new("NominationFile.FilePathEmpty", "مسار الملف لا يمكن أن يكون فارغًا.");

        public static readonly Error FilePathInvalid =
            new("NominationFile.FilePathInvalid", "مسار الملف غير صالح.");

        public static readonly Error ReferenceNumberEmpty =
            new("NominationFile.ReferenceNumberEmpty", "رقم المرجع لا يمكن أن يكون فارغًا.");

        public static readonly Error ReferenceNumberDuplicate =
            new("NominationFile.ReferenceNumberDuplicate", "رقم المرجع مستخدم من قبل.");

        public static readonly Error CandidateIdInvalid =
            new("NominationFile.CandidateIdInvalid", "معرف المرشح غير صالح.");

        public static readonly Error ExpectedEndDateInvalid =
            new("NominationFile.ExpectedEndDateInvalid", "تاريخ الانتهاء المتوقع غير صحيح.");

        public static readonly Error InvalidStatusTransition =
            new("NominationFile.InvalidStatusTransition", "لا يمكن تغيير حالة الملف لهذا الوضع.");
    }
}
