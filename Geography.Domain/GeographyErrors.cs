using Modules.Shared.Domain;

namespace Geography.Domain
{
    public static class GeoErrors
    {
        // Validation Errors
        public static readonly Error NameEmpty =
            new("Geo.NameEmpty", "الاسم مطلوب");

        public static readonly Error CodeEmpty =
            new("Geo.CodeEmpty", "الكود مطلوب");

        public static readonly Error GovernorateIdEmpty =
            new("Geo.GovernorateIdEmpty", "المحافظة مطلوبة");

        public static readonly Error CityCenterIdEmpty =
            new("Geo.CityCenterIdEmpty", "المركز/المدينة مطلوب");

        public static readonly Error LocalUnitIdEmpty =
            new("Geo.LocalUnitIdEmpty", "الوحدة المحلية مطلوبة");

        // Business Rule & Not Found Errors
        public static readonly Error GovernorateNotFound =
            new("Geo.GovernorateNotFound", "المحافظة غير موجودة");

        public static readonly Error GovernorateCodeExists =
            new("Geo.GovernorateCodeExists", "كود المحافظة مسجل مسبقاً");

        public static readonly Error CityCenterNotFound =
            new("Geo.CityCenterNotFound", "المركز/المدينة غير موجود");

        public static readonly Error LocalUnitNotFound =
            new("Geo.LocalUnitNotFound", "الوحدة المحلية غير موجودة");

        public static readonly Error VillageNotFound =
            new("Geo.VillageNotFound", "القرية غير موجودة");
    }
}
