using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Entities
{
    public static class PromotionErrors
    {
        public static readonly Error OutRangeOfScore =
            new("Promotion.OutRangeOfScore", "لا يجب ان يكون النتيجه تحت الصفر ولا فوق المائة ");

        public static readonly Error AlreadyIsApproved =
            new("Promotion.AlreadyIsApproved", "الكشف معتمد بالفعل ولا يمكن اعتماده مرة أخرى");

        public static readonly Error MustSelectOneAtLeast =
            new("Promotion.MustSelectOneAtLeast", "يجب اختيار موظف واحد على الأقل قبل الاعتماد");

        public static readonly Error IsExclused =
            new("Promotion.IsExclused", "الموظف مستبعد من الترقية ولا يمكن اختياره");

        public static readonly Error EmployeeRequired =
            new("Promotion.EmployeeRequired", "الموظف مطلوب");

        public static readonly Error InvalidGradeChange =
            new("Promotion.InvalidGradeChange", "التغيير المقترح غير صالح، يجب أن يكون أعلى من الدرجة الحالية");

        public static readonly Error InvalidSubScore =
            new("Promotion.InvalidSubScore", "الدرجة الفرعية خارج النطاق المسموح");
    }
}
