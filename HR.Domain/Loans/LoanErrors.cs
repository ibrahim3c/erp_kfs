using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public static class LoanErrors
    {
        public static readonly Error InvalidAmount = 
            new Error("Loan.InvalidAmount", "قيمة السلفة يجب أن تكون أكبر من صفر");
       
        public static readonly Error InvalidMonths =
            new Error("Loan.InvalidMonths", "عدد الأشهر يجب أن يكون شهر أو أكثر");
       
        public static readonly Error NotFoundLoans = 
            new Error("Loan.NotFoundLoans", "لم يتم العثور على أي سلف");
        
        public static readonly Error NotFoundLoan =
            new Error("Loan.NotFoundLoan", "لم يتم العثور على السلفة المطلوبة");

        public static readonly Error LoanAlreadyCompleted =
            new("Loan.AlreadyCompleted", "السلفة خالصة بالفعل");

        public static readonly Error NoRemainingInstallments =
            new("Loan.NoRemainingInstallments", "لا توجد أقساط متبقية");

        public static readonly Error InstallmentGreaterThanZero =
            new("Loan.InstallmentGreaterThanZero", "قيمة القسط يجب أن تكون أكبر من صفر");
    }
}
