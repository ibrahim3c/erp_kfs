using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Payrolls
{
    public static class PayrollErrors
    {
        public static readonly Error InvalidMonth =
            new("Payroll.InvalidMonth", "الشهر يجب أن يكون بين 1 و 12");

        public static readonly Error InvalidYear =
            new("Payroll.InvalidYear", "السنة غير صحيحة");

        public static readonly Error CycleLocked =
            new("Payroll.CycleLocked", "الدورة مقفولة ولا يمكن التعديل عليها");

        public static readonly Error CycleNotReady =
            new("Payroll.CycleNotReady", "يجب حساب الرواتب أولاً قبل الإقفال");

        public static readonly Error CycleNotFound =
            new("Payroll.CycleNotFound", "الدورة غير موجودة");

        public static readonly Error EntryNotFound =
            new("Payroll.EntryNotFound", "مفردات الراتب غير موجودة");

        public static readonly Error InvalidAdjustmentAmount =
            new("Payroll.InvalidAdjustmentAmount", "المبلغ يجب أن يكون أكبر من صفر");

        public static readonly Error AdjustmentReasonRequired =
            new("Payroll.AdjustmentReasonRequired", "سبب التسوية مطلوب");
    }
}
