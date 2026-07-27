using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.ServiceTerms.Enums
{
    public enum ServiceTermStatus
    {
        UnderReview = 1,   // تحت المراجعة
        Approved = 2,      // تم الضم
        Rejected = 3       // مرفوض
    }
}
