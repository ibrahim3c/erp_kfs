using Modules.Shared.Domain;

namespace HR.Domain.Funds
{
    public static class FundErrors
    {
        public static readonly Error EmployeeRequired =
            new("Fund.EmployeeRequired", "يجب اختيار موظف");

        public static readonly Error SubscriptionDateRequired =
            new("Fund.SubscriptionDateRequired", "تاريخ الاشتراك مطلوب");

        public static readonly Error FundTypeRequired =
            new("Fund.FundTypeRequired", "يجب تحديد نوع الصندوق");

        public static readonly Error AlreadySubscribed =
            new("Fund.AlreadySubscribed", "هذا الموظف مشترك بالفعل في الصندوق");

        public static readonly Error NotSubscribed =
            new("Fund.NotSubscribed", "هذا الموظف غير مشترك في الصندوق");

        public static readonly Error ClaimTypeRequired =
            new("Fund.ClaimTypeRequired", "نوع المطالبة مطلوب");

        public static readonly Error EventDateRequired =
            new("Fund.EventDateRequired", "تاريخ الحدث مطلوب");

        public static readonly Error DuplicateSubscription =
            new("Fund.DuplicateSubscription", "يوجد اشتراك نشط بالفعل لهذا الصندوق");
    }
}
