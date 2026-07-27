using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.ServiceTerms.Entities
{
    public static class ServiceTermErrors
    {
        public static readonly Error InvalidEntity = new Error("ServiceTerm.InvalidEntity", "الجهة السابقة مطلوبة");
        public static readonly Error InvalidDates = new Error("ServiceTerm.InvalidDates", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
        public static readonly Error InvalidStatus = new Error("ServiceTerm.InvalidStatus", "لا يمكن اعتماد طلب ليس تحت المراجعة");
        public static readonly Error ReasonRequired = new Error("ServiceTerm.ReasonRequired", "سبب الرفض مطلوب");

        public static readonly Error NotFound = new Error("ServiceTerm.NotFound", "طلب الخدمة غير موجود");

        public static readonly Error EmployeeNotFound = new Error("ServiceTerm.EmployeeNotFound", "الموظف غير موجود");
    }
}
