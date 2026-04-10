using Modules.Shared.Domain;

namespace HR.Domain.Candidates
{
    public static class CandidateErrors
    {
        public static readonly Error FullNameEmpty =
            new("Candidate.FullNameEmpty", "الاسم بالكامل لا يمكن أن يكون فارغًا.");

        public static readonly Error FullNameTooLong =
            new("Candidate.FullNameTooLong", "الاسم بالكامل يتجاوز الحد الأقصى المسموح به.");

        public static readonly Error NationalIdEmpty =
            new("Candidate.NationalIdEmpty", "الرقم القومي لا يمكن أن يكون فارغًا.");

        public static readonly Error NationalIdInvalid =
            new("Candidate.NationalIdInvalid", "صيغة الرقم القومي غير صحيحة.");

        public static readonly Error NationalIdDuplicate =
            new("Candidate.NationalIdDuplicate", "يوجد مرشح مسجل بنفس الرقم القومي.");

        public static readonly Error PhoneEmpty =
            new("Candidate.PhoneEmpty", "رقم الهاتف لا يمكن أن يكون فارغًا.");

        public static readonly Error PhoneInvalid =
            new("Candidate.PhoneInvalid", "صيغة رقم الهاتف غير صحيحة.");

        public static readonly Error EmailInvalid =
            new("Candidate.EmailInvalid", "صيغة البريد الإلكتروني غير صحيحة.");

        public static readonly Error QualificationRequired =
            new("Candidate.QualificationRequired", "المؤهل الدراسي مطلوب.");

        public static readonly Error CityCenterRequired =
            new("Candidate.CityCenterRequired", "المركز أو المدينة مطلوب.");

        public static readonly Error VillageRequired =
            new("Candidate.VillageRequired", "القرية مطلوبة.");

        public static readonly Error CandidateNotFound =
            new("Candidate.NotFound", "لم يتم العثور على بيانات المرشح.");

        public static readonly Error CandidateInactive =
            new("Candidate.Inactive", "المرشح غير نشط.");

        public static readonly Error DuplicateNominationFile =
            new("Candidate.DuplicateNominationFile", "تمت إضافة هذا الملف مسبقًا.");

        public static readonly Error NominationFileNotFound =
            new("Candidate.NominationFileNotFound", "لم يتم العثور على ملف الترشح.");

        public static readonly Error CannotDeactivate =
            new("Candidate.CannotDeactivate", "لا يمكن تعطيل المرشح لارتباطه ببيانات أو إجراءات أخرى.");
    }
}