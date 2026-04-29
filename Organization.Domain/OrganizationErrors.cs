using Modules.Shared.Domain;

namespace Organization.Domain
{
    public static class OrganizationErrors
    {
        public static readonly Error OrgUnitIdEmpty = new("Leadership.OrgUnitIdEmpty", "الهيكل التنظيمي مطلوب");
        public static readonly Error JobTitleIdEmpty = new("Leadership.JobTitleIdEmpty", "المسمى الوظيفي مطلوب");
        public static readonly Error LeadershipAlreadyActive = new("Leadership.AlreadyActive", "المنصب نشط بالفعل");
        public static readonly Error LeadershipAlreadyInactive = new("Leadership.AlreadyInactive", "المنصب غير نشط بالفعل");
        public static readonly Error EndDateInvalid = new("LeadershipHistory.EndDateInvalid", "تاريخ نهاية المنصب يجب أن يكون بعد تاريخ البداية");

        // OrgUnitType
        public static readonly Error OrgUnitTypeNotFound = new("OrgUnitType.NotFound", "نوع الوحدة التنظيمية غير موجود");
        public static readonly Error CodeRequired = new("OrgUnitType.CodeRequired", "كود نوع الوحدة التنظيمية مطلوب");
        public static readonly Error NameRequired = new("OrgUnitType.NameRequired", "اسم نوع الوحدة التنظيمية مطلوب");
        public static readonly Error InvalidLevelOrder = new("OrgUnitType.InvalidLevelOrder", "ترتيب المستوى غير صالح");

        // OrgUnit
        public static readonly Error OrgUnitNotFound = new("OrgUnit.NotFound", "الوحدة التنظيمية غير موجودة");
        public static readonly Error OrgUnitTypeIdEmpty = new("OrgUnit.OrgUnitTypeIdEmpty", "نوع الوحدة التنظيمية مطلوب");

        // LeadershipPosition
        public static readonly Error LeadershipPositionNotFound = new("LeadershipPosition.NotFound", "المنصب القيادي غير موجود");

        // LeadershipPositionHistory
        public static readonly Error LeadershipPositionHistoryNotFound = new("LeadershipPositionHistory.NotFound", "سجل المنصب القيادي غير موجود");

        // JobStructure - QualitativeGroup
        public static readonly Error CodeEmpty = new("QualitativeGroup.CodeEmpty", "كود المجموعة الوظيفية مطلوب");
        public static readonly Error NameEmpty = new("QualitativeGroup.NameEmpty", "اسم المجموعة الوظيفية مطلوب");
        public static readonly Error AlreadyActive = new("QualitativeGroup.AlreadyActive", "المجموعة نشطة بالفعل");
        public static readonly Error AlreadyInactive = new("QualitativeGroup.AlreadyInactive", "المجموعة غير نشطة بالفعل");

        // JobStructure - FunctionalGroup
        public static readonly Error QualitativeGroupIdEmpty = new("FunctionalGroup.QualitativeGroupIdEmpty", "المجموعة الوظيفية مطلوبة");

        // JobStructure - JobTitle
        public static readonly Error FunctionalGroupIdEmpty = new("JobTitle.FunctionalGroupIdEmpty", "المجموعة الوظيفية مطلوبة");

        // JobStructure - JobGrade
        public static readonly Error InvalidGradeLevel = new("JobGrade.InvalidGradeLevel", "مستوى الدرجة غير صالح");
        public static readonly Error InvalidYearsNo = new("JobGrade.InvalidYearsNo", "عدد السنوات غير صالح");
    }
}
