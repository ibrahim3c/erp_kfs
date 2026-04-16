using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Penalties
{
    public static class PenaltyErrors
    {
        public static readonly Error EmployeeEmpty = new("Penalty.EmployeeEmpty", "يجب تحديد الموظف.");
        public static readonly Error TypeEmpty = new("Penalty.TypeEmpty", "يجب تحديد نوع المخالفة.");
        public static readonly Error NameEmpty = new Error("PenaltyType.NameEmpty", "اسم المخالفة مطلوب.");
        public static readonly Error InvalidDays = new("Penalty.InvalidDays", "يجب إدخال عدد أيام الخصم.");
        public static readonly Error NotFound = new("Penalty.NotFound", "المخالفة غير موجودة.");
        public static readonly Error AllNotFound = new("Penalty.AllNotFound", "لا يوجد مخالفات.");
    }
}
