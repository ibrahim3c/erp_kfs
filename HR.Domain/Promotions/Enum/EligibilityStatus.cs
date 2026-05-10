using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Enum
{
    public enum EligibilityStatus
    {
        Pending = 0,  // لم يُفحص بعد
        Eligible = 1,  // مستحق ✅
        Excluded = 2   // مستبعد ❌
    }
}
