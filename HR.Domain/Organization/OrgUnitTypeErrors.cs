using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Organization
{
    public static class OrgUnitTypeErrors
    {
        public static readonly Error CodeEmpty =
       new("OrgUnitType.CodeEmpty", "كود نوع الوحدة لا يمكن أن يكون فارغًا.");

        public static readonly Error CodeTooLong =
            new("OrgUnitType.CodeTooLong", "كود نوع الوحدة يتجاوز الحد الأقصى المسموح به.");

        public static readonly Error NameEmpty =
            new("OrgUnitType.NameEmpty", "اسم نوع الوحدة لا يمكن أن يكون فارغًا.");

        public static readonly Error NameTooLong =
            new("OrgUnitType.NameTooLong", "اسم نوع الوحدة يتجاوز الحد الأقصى المسموح به.");

        public static readonly Error LevelOrderInvalid =
            new("OrgUnitType.LevelOrderInvalid", "ترتيب المستوى يجب أن يكون رقمًا موجبًا أو صفر.");

        public static readonly Error DuplicateCode =
            new("OrgUnitType.DuplicateCode", "يوجد نوع وحدة بنفس الكود بالفعل.");
    }
}
