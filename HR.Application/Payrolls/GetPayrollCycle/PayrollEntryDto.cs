using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayrollCycle
{
    public class PayrollEntryDto
    {
        public Guid EntryId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public decimal BasicSalary { get; init; }
        public decimal Incentives { get; init; }
        public decimal Allowances { get; init; }
        public decimal GrossSalary { get; init; }
        public decimal TotalDeductions { get; init; }
        public decimal NetSalary { get; init; }
        public int HasPenalty { get; init; }
        public decimal PenaltyDeduction { get; init; }
    }
}
