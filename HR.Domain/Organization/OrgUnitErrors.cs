using Modules.Shared.Domain;


namespace HR.Domain.Organization
{
    public static class OrgUnitErrors
    {
        public static readonly Error NameEmpty =
            new("OrgUnit.NameEmpty", "اسم الوحدة التنظيمية لا يمكن أن يكون فارغًا.");

        public static readonly Error NameTooLong =
            new("OrgUnit.NameTooLong", "اسم الوحدة التنظيمية يتجاوز الحد الأقصى المسموح به.");

        public static readonly Error CodeEmpty =
            new("OrgUnit.CodeEmpty", "كود الوحدة التنظيمية لا يمكن أن يكون فارغًا.");

        public static readonly Error CodeTooLong =
            new("OrgUnit.CodeTooLong", "كود الوحدة التنظيمية يتجاوز الحد الأقصى المسموح به.");

        public static readonly Error OrgUnitTypeRequired =
            new("OrgUnit.OrgUnitTypeRequired", "نوع الوحدة التنظيمية مطلوب.");

        public static readonly Error ParentInvalid =
            new("OrgUnit.ParentInvalid", "الوحدة الأب غير صالحة.");

        public static readonly Error CannotBeSelfParent =
            new("OrgUnit.CannotBeSelfParent", "لا يمكن أن تكون الوحدة أبًا لنفسها.");

        public static readonly Error DuplicateCode =
            new("OrgUnit.DuplicateCode", "يوجد وحدة تنظيمية بنفس الكود بالفعل.");

        public static readonly Error InvalidHierarchyLevel =
            new("OrgUnit.InvalidHierarchyLevel", "لا يمكن ربط الوحدة بهذا المستوى التنظيمي.");
    }
}
