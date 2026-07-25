using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Enums
{
    public enum RetirementStage
    {
        PendingReview = 1,          // مراجعة التدرج الوظيفي
        UnderFinancialReview = 2,   // تحت المراجعة المالية
        AwaitingSignatures = 3,     // في انتظار التوقيعات
        DeliveredToAuthority = 4,   // تم التسليم للهيئة
        Rejected = 5                // ملفات مرتدة
    }
}
