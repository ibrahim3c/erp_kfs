using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Terminations
{
    public static class TerminationErrors
    {
        public static readonly Error NotFound = new Error("Termination.NotFound", "قرار الإنهاء غير موجود");
        public static readonly Error InvalidDecisionNumber = new Error("Termination.InvalidDecisionNumber", "رقم القرار مطلوب");
        public static readonly Error InvalidDates = new Error("Termination.InvalidDates", "تاريخ آخر يوم عمل غير منطقي مقارنة بتاريخ القرار");
        public static readonly Error AlreadyCancelled = new Error("Termination.AlreadyCancelled", "القرار ملغى بالفعل");
        public static readonly Error AlreadyExecuted = new Error("Termination.AlreadyExecuted", "القرار تم تنفيذه بالفعل");
        public static readonly Error CancellationReasonRequired = new Error("Termination.CancellationReasonRequired", "سبب الإلغاء مطلوب");
    }
}
