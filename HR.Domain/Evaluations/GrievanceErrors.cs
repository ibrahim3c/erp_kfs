using Modules.Shared.Domain;

namespace HR.Domain.Evaluations
{
    public static class GrievanceErrors
    {
        public static readonly Error EmployeeRequired =
            new("Grievance.EmployeeRequired", "الموظف مطلوب");

        public static readonly Error GrievanceTypeRequired =
            new("Grievance.TypeRequired", "نوع التظلم مطلوب");

        public static readonly Error DecisionNumberRequired =
            new("Grievance.DecisionNumberRequired", "رقم القرار مطلوب");

        public static readonly Error ReasonsRequired =
            new("Grievance.ReasonsRequired", "أسباب التظلم مطلوبة");

        public static readonly Error GrievanceNotFound =
            new("Grievance.NotFound", "التظلم غير موجود");

        public static readonly Error AlreadyResolved =
            new("Grievance.AlreadyResolved", "تم البت في هذا التظلم مسبقاً");

        public static readonly Error InvalidResolutionDate =
            new("Grievance.InvalidResolutionDate", "تاريخ البت يجب أن يكون بعد تاريخ التقديم");
    }
}
