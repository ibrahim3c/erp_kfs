using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Transfers.Entities
{
    public static class TranseferErrors
    {
        public static readonly Error SameDepartment = new Error("Transfer.SameDepartment", "لا يمكن النقل لنفس الإدارة");
        public static readonly Error ReasonRequired = new Error("Transfer.ReasonRequired", "سبب النقل مطلوب");
        public static readonly Error InvalidStatus = new Error("Transfer.InvalidStatus", "لا يمكن اعتماد حركة تم اعتمادها بالفعل");
        public static readonly Error NotFoundInternal = new Error("Transfer.NotFoundInternal", "لم يتم العثور على حركة النقل الداخلي");
        public static readonly Error NotFoundExternal = new Error("Transfer.NotFoundExternal", "لم يتم العثور على حركة النقل الخارجية");

        // External Movement Errors
        public static readonly Error InvalidEntity = new Error("ExternalMovement.InvalidEntity", "اسم الجهة الأخرى مطلوب");
        public static readonly Error DatesRequired = new Error("ExternalMovement.DatesRequired", "تاريخ البداية والنهاية مطلوبان للندب");
        public static readonly Error InvalidDates = new Error("ExternalMovement.InvalidDates", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
        public static readonly Error SalaryBearerRequired = new Error("ExternalMovement.SalaryBearerRequired", "جهة تحمل الراتب مطلوبة للندب");
        public static readonly Error InvalidMovementType = new Error("ExternalMovement.InvalidMovementType", "نوع الحركة الخارجية غير صالح");
        public static readonly Error NotSecondment = new Error("ExternalMovement.NotSecondment", "الحركة الخارجية ليست ندبًا");
        public static readonly Error NotActive = new Error("ExternalMovement.NotActive", "لا يمكن تجديد حركة غير سارية");
        public static readonly Error AlreadyEnded = new Error("ExternalMovement.AlreadyEnded", "لا يمكن تجديد حركة انتهت بالفعل");
        public static readonly Error InvalidRenewalDate = new Error("ExternalMovement.InvalidRenewalDate", "تاريخ التجديد يجب أن يكون بعد التاريخ الحالي");

    }
}
