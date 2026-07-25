using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Entities
{
    public static class RetirementErrors
    {
        public static readonly Error InvalidRetirementReason =
            new("Retirement.InvalidRetirementReason", "سبب التقاعد غير صالح");

        public static readonly Error InvalidEmployee =
            new("Retirement.InvalidEmployee", "الموظف غير صالح");

        public static readonly Error ChecklistIncomplete = 
            new("Retirement.ChecklistIncomplete", "قائمة التحقق غير مكتملة");

        public static readonly Error NotFound =
            new("Retirement.NotFound", "ملف المعاش غير موجود");
    }
}
