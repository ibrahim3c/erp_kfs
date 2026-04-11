using Modules.Shared.Domain;

namespace HR.Domain.Employees
{
    public static class EmployeeErrors
    {
        public static readonly Error AlreadyInactive = new Error("Employee.AlreadyInactive", "الموظف غير نشط بالفعل");
        public static readonly Error CodeEmpty = new Error(
            "Employee.CodeEmpty",
            "كود الموظف مطلوب ولا يمكن أن يكون فارغاً");

        public static readonly Error NameEmpty = new Error(
            "Employee.NameEmpty",
            "اسم الموظف مطلوب ولا يمكن أن يكون فارغاً");

        public static readonly Error InvalidNationalId = new Error(
            "Employee.InvalidNationalId",
            "الرقم القومي غير صحيح، يجب أن يتكون من 14 رقماً");

        public static readonly Error InvalidHireDate = new Error(
            "Employee.InvalidHireDate",
            "تاريخ التعيين غير صحيح");

        public static readonly Error InvalidTerminationDate = new Error(
            "Employee.InvalidTerminationDate",
            "تاريخ إنهاء الخدمة لا يمكن أن يكون قبل تاريخ التعيين");

        public static readonly Error Inactive = new Error(
            "Employee.Inactive",
            "لا يمكن إجراء هذه العملية لأن الموظف غير نشط");

        public static readonly Error EmployeeIdEmpty = new Error(
            "Employee.EmployeeIdEmpty",
            "معرف الموظف مطلوب لربط فرد العائلة به");

        public static readonly Error FullNameEmpty = new Error(
            "Employee.FullNameEmpty",
            "اسم فرد العائلة مطلوب ولا يمكن أن يكون فارغاً");

        public static readonly Error RelationshipTypeEmpty = new Error(
            "Employee.RelationshipTypeEmpty",
            "صلة القرابة مطلوبة (مثل: زوجة، ابن، ابنة)");

        public static readonly Error AlreadyDisabled = new Error(
            "Employee.AlreadyDisabled",
            "فرد العائلة مسجل كذوي همم بالفعل");

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



        public static readonly Error TerminationTypeIdEmpty = new Error(
    "ServiceTermination.TerminationTypeIdEmpty",
    "نوع إنهاء الخدمة مطلوب");

        public static readonly Error RequestNumberEmpty = new Error(
            "ServiceTermination.RequestNumberEmpty",
            "رقم الطلب مطلوب");

        public static readonly Error InvalidRequestDate = new Error(
            "Employee.InvalidRequestDate",
            "تاريخ الطلب غير صحيح");

        public static readonly Error InvalidRequestStartDate = new Error(
            "ServiceTermination.InvalidRequestStartDate",
            "تاريخ بدء إنهاء الخدمة يجب أن يكون بعد تاريخ الطلب");

        public static readonly Error AlreadyCancelledTerminationRequest = new Error(
            "ServiceTermination.AlreadyCancelled",
            "طلب إنهاء الخدمة ملغي بالفعل");

        public static readonly Error AlreadyApprovedTerminationRequest = new Error(
            "ServiceTermination.AlreadyApproved",
            "طلب إنهاء الخدمة تمت الموافقة عليه بالفعل");


        public static readonly Error AcademicIncentiveTypeIdEmpty = new Error(
    "AcademicIncentive.TypeIdEmpty",
    "نوع الحافز العلمي مطلوب");

        public static readonly Error QualificationIdEmpty = new Error(
            "AcademicIncentive.QualificationIdEmpty",
            "المؤهل العلمي مطلوب");


        public static readonly Error InvalidAffectDate = new Error(
            "AcademicIncentive.InvalidAffectDate",
            "تاريخ تأثير الطلب يجب أن يكون بعد تاريخ الطلب");

        public static readonly Error RequestAlreadySubmitted = new Error(
            "AcademicIncentive.AlreadySubmitted",
            "تم إرسال الطلب بالفعل");

        public static readonly Error RequestAlreadyApproved = new Error(
            "AcademicIncentive.AlreadyApproved",
            "تمت الموافقة على الطلب بالفعل");

        public static readonly Error RequestAlreadyRejected = new Error(
            "AcademicIncentive.AlreadyRejected",
            "تم رفض الطلب بالفعل");

        public static readonly Error TerminationTypeCodeEmpty = new Error(
    "ServiceTerminationType.CodeEmpty",
    "كود نوع إنهاء الخدمة مطلوب");

        public static readonly Error TerminationTypeNameEmpty = new Error(
            "ServiceTerminationType.NameEmpty",
            "اسم نوع إنهاء الخدمة مطلوب");

        public static readonly Error AlreadyActive = new Error(
            "ServiceTerminationType.AlreadyActive",
            "النوع نشط بالفعل");



        public static readonly Error IncentiveCodeEmpty =
    new("AcademicIncentiveType.CodeEmpty", "كود الحافز العلمي مطلوب");

        public static readonly Error IncentiveNameEmpty =
            new("AcademicIncentiveType.NameEmpty", "اسم الحافز العلمي مطلوب");

        public static readonly Error IncentiveInvalidValue =
            new("AcademicIncentiveType.InvalidValue", "قيمة الحافز يجب أن تكون أكبر من صفر");

        public static readonly Error IncentiveInvalidValueType =
            new("AcademicIncentiveType.InvalidValueType", "يجب تحديد نوع القيمة (نسبة أو مبلغ)");

        public static readonly Error IncentiveAlreadyInactive =
            new("AcademicIncentiveType.AlreadyInactive", "الحافز غير نشط بالفعل");

        public static readonly Error IncentiveAlreadyActive =
            new("AcademicIncentiveType.AlreadyActive", "الحافز نشط بالفعل");

        public static readonly Error EmployeeFileRequired = new Error(
    "EmployeeFile.Required",
    "يجب رفع ملف واحد على الأقل للموظف");

        public static readonly Error InvalidFilePath = new Error(
            "EmployeeFile.InvalidFilePath",
            "مسار الملف غير صالح");



        public static readonly Error QualificationFullNameEmpty = new Error(
            "EmployeeQualification.QualificationFullNameEmpty",
            "اسم المؤهل الكامل مطلوب");

        public static readonly Error InvalidQualificationDates = new Error(
            "EmployeeQualification.InvalidDates",
            "تاريخ الانتهاء يجب أن يكون بعد تاريخ البداية");

        public static readonly Error AlreadyVerifiedQualification = new Error(
            "EmployeeQualification.AlreadyVerified",
            "المؤهل تم التحقق منه بالفعل");


    }
}