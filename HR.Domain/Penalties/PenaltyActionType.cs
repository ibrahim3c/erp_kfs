using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Penalties
{
    public enum PenaltyActionType
    {
        Warning = 1,     // إنذار
        Deduct = 2,      // خصم من الراتب
        Hold = 3,        // وقف عن العمل
        Postpone = 4     // تأجيل ترقية
    }
}
