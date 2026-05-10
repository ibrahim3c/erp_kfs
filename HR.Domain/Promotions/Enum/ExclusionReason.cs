using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Enum
{
    public enum ExclusionReason
    {
        None = 0,  // لا يوجد — الموظف مستحق
        ExceededPenalties = 1, // تجاوز 10 أيام جزاء
        LowKpiScore = 2,  // تقرير كفاءة أقل من المطلوب
        InsufficientYears = 3, // لم يكمل المدة المطلوبة في الدرجة
        AlreadyMaxGrade = 4,  // على أعلى درجة ولا ترقية فوقها
        IncentiveQuotaFull = 5, // تجاوز حصة 10% في الدرجة
        InvalidGradeChange = 6,
    }
}
