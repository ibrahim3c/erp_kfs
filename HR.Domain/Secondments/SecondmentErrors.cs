using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Secondments
{
    public static class SecondmentErrors
    {
        public static readonly Error NotFound =
            new("Secondment.NotFound", "الإعارة غير موجودة");
        public static readonly Error InvalidHostEntity = 
            new("Secondment.InvalidHostEntity", "الجهة المنتدب إليها مطلوبة");

        public static readonly Error InvalidDate =
            new("Secondment.InvalidDate", "تاريخ الإعارة غير صالح");

        public static readonly Error InvalidSalaryBearer =
            new("Secondment.InvalidSalaryBearer", "الجهة التي تتحمل الراتب مطلوبة");

        public static readonly Error CannotRenew =
            new("Secondment.CannotRenew", "لا يمكن تجديد الإعارة");

        public static readonly Error InvalidRenewalDate = 
            new("Secondment.InvalidRenewalDate", "تاريخ التجديد غير صالح");

        public static readonly Error AlreadyEnded = 
            new("Secondment.AlreadyEnded", "الإعارة قد انتهت بالفعل");

        public static readonly Error AlreadyActive = 
            new("Secondment.AlreadyActive", "الإعارة قد بدأت بالفعل");

        public static readonly Error ClearanceRequired = 
            new("Secondment.ClearanceRequired", "مطلوب إنهاء الإعارة قبل تجديدها");
    }
}
