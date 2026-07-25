using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Secondments.Enums
{
    public enum SalaryBearer
    {
        OriginalEntity = 1,  // جهتنا
        HostEntity = 2,      // الجهة المستعيرة
        Unpaid = 3           // بدون راتب
    }
}
