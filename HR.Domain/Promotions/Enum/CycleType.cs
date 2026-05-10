using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Enum
{
    public enum CycleType
    {
        Promotion = 1,   // ترقية درجة — 4 سنوات في الدرجة
        Periodic = 2,   // علاوة دورية 7% — كل سنة في 1/7
        Incentive = 3    // علاوة تشجيعية 10% — أفضل 10% بتقدير ممتاز
    }
}
