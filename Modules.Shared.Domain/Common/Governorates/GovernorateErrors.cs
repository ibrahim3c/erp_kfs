using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Shared.Domain.Common.Governorates
{
    public static class GovernorateErrors
    {
        public static readonly Error NameEmpty =
            new("Governorate.NameEmpty", "الاسم لا يمكن أن يكون فارغًا.");

        public static readonly Error CodeEmpty =
            new("Governorate.CodeEmpty", "الكود لا يمكن أن يكون فارغًا.");

    }
}
