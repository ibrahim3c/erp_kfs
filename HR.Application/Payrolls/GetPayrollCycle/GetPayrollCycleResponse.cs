using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayrollCycle
{
    public class GetPayrollCycleResponse
    {
        public Guid CycleId { get; init; }
        public int Month { get; init; }
        public int Year { get; init; }
        public string Status { get; init; } = string.Empty;
        public int EmployeeCount { get; init; }
        public decimal TotalDeductions { get; init; }
        public decimal TotalNetSalary { get; init; }
        public List<PayrollEntryDto> Entries { get; init; } = new();
    }
}
