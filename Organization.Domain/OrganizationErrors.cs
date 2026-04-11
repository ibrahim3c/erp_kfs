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

    }
}
