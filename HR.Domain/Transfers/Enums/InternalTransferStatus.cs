using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Transfers.Enums
{

    public enum InternalTransferStatus
    {
        PendingApproval = 1,   // بانتظار المدير
        Approved = 2           // تم النقل
    }
}
