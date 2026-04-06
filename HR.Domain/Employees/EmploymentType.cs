using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Employees
{
    public enum EmploymentType
    {
        Competition = 1, // تعيين بمسابقة
        TemporaryContract = 2, // تعاقد مؤقت
        BudgetBand = 3, // بند موازنة
        DailyWage = 4 // يومية/سركي
    }
}
