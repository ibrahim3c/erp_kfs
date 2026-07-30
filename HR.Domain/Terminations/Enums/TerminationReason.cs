using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Terminations.Enums
{
    public enum TerminationReason
    {
        Resignation = 1,   // استقالة مكتوبة
        Dismissal = 2,     // فصل (تأديبي / قضائي)
        Absence = 3,       // فصل للانقطاع
        Death = 4,         // الوفاة
        Medical = 5,       // عدم اللياقة الصحية
        Contract = 6       // انتهاء مدة العقد
    }
}
