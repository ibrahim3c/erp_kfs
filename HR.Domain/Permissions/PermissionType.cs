using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{

    public enum PermissionType
    {
        Personal = 1,  // إذن شخصي — يخصم من الرصيد
        Official = 2,  // مأمورية عمل — لا يخصم
        Medical = 3   // إذن مرضي — عيادة
    }
}
