using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Enums
{
    public enum RetirementReason
    {
        LegalAge = 1,     // بلوغ السن القانوني
        Death = 2,        // الوفاة أثناء الخدمة
        Resignation = 3,  // استقالة
        Disability = 4    // عجز
    }
}
