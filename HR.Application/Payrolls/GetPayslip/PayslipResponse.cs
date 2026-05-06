using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayslip
{
    // PayslipResponse.cs
    public class PayslipResponse
    {
        public string EmployeeName { get; init; } = string.Empty;
        public string EmployeeCode { get; init; } = string.Empty;
        public string JobTitle { get; init; } = string.Empty;
        public int Month { get; init; }
        public int Year { get; init; }

        public decimal BasicSalary { get; init; }
        public decimal Incentives { get; init; }
        public decimal Allowances { get; init; }
        public decimal ManualAdditions { get; init; }
        public decimal GrossSalary { get; init; }

        public decimal InsuranceDeduction { get; init; }
        public decimal TaxDeduction { get; init; }
        public decimal LoanDeduction { get; init; }
        public decimal InsurancePurchaseDeduction { get; init; }
        public decimal PenaltyDeduction { get; init; }
        public decimal ManualDeductions { get; init; }
        public decimal TotalDeductions { get; init; }

        public decimal NetSalary { get; init; }
        public string BankName { get; init; } = string.Empty;
        public string BankAccount { get; init; } = string.Empty;
    }
}
