using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public enum InsurancePurchaseStatus
    {
        PendingApproval = 1,  // معلق — في انتظار الاعتماد
        Approved = 2,         // معتمد وسارى
        Rejected = 3,         // مرفوض
        Completed = 4         // منتهى — تم السداد الكامل
    }
}
