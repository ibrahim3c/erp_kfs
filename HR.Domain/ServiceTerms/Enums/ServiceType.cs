using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.ServiceTerms.Enums
{
    public enum ServiceType
    {
        Government = 1,       // مدة حكومية (ضم كامل)
        PrivateSector = 2,    // قطاع خاص (خاضع للتأمينات)
        Conscription = 3      // مدة تجنيد / خدمة عامة
    }
}
