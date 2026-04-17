using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public static class InsurancePurchaseErrors
    {
        public static readonly Error InvalidEmployee =
            new("InsurancePurchase.InvalidEmployee", "الموظف غير صحيح");

        public static readonly Error InvalidAuthority =
            new("InsurancePurchase.InvalidAuthority", "الجهة التأمينية مطلوبة");

        public static readonly Error InvalidYears =
            new("InsurancePurchase.InvalidYears", "عدد السنوات يجب أن يكون أكبر من صفر");

        public static readonly Error InvalidTotalCost =
            new("InsurancePurchase.InvalidTotalCost", "التكلفة الإجمالية يجب أن تكون أكبر من صفر");

        public static readonly Error InvalidMonthlyInstallment =
            new("InsurancePurchase.InvalidMonthlyInstallment", "القسط الشهري غير صحيح");

        public static readonly Error InstallmentExceedsTotalCost =
            new("InsurancePurchase.InstallmentExceedsTotalCost", "القسط الشهري أكبر من التكلفة الإجمالية");

        public static readonly Error AlreadyProcessed =
            new("InsurancePurchase.AlreadyProcessed", "الطلب تمت معالجته بالفعل");

        public static readonly Error NotActive =
            new("InsurancePurchase.NotActive", "الطلب غير نشط أو مكتمل بالفعل");

        public static readonly Error NotFound =
            new("InsurancePurchase.NotFound", "طلب شراء التأمين غير موجود");
    }
}
