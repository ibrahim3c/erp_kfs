

namespace Modules.Shared.Domain.Common.City_Center
{
    public class CityCenterErrors
    {
        public static readonly Error NameEmpty =
          new("CityCenter.NameEmpty", "اسم المركز أو المدينة لا يمكن أن يكون فارغًا.");

        public static readonly Error NameTooLong =
            new("CityCenter.NameTooLong", "اسم المركز أو المدينة طويل جدًا.");

        public static readonly Error TypeEmpty =
            new("CityCenter.TypeEmpty", "نوع المركز مطلوب.");

        public static readonly Error InvalidType =
            new("CityCenter.InvalidType", "نوع المركز يجب أن يكون (center | city) فقط.");

        public static readonly Error GovernorateRequired =
            new("CityCenter.GovernorateRequired", "المحافظة مطلوبة.");

        public static readonly Error DuplicateName =
            new("CityCenter.DuplicateName", "يوجد مركز أو مدينة بنفس الاسم بالفعل.");

        public static readonly Error InvalidId =
            new("CityCenter.InvalidId", "الـ Id غير صالح.");
    }
}

