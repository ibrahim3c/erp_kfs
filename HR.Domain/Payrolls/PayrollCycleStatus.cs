using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Payrolls
{
    public enum PayrollCycleStatus
    {
        Draft = 1,  // مسودة
        UnderReview = 2,  // قيد المراجعة
        Locked = 3   // مقفول
    }
}
