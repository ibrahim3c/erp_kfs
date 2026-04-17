using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetInsurancePurchaseList
{
    public sealed class GetInsurancePurchaseListResponse
    {
        public Guid Id { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string InsuranceAuthority { get; init; } = string.Empty;
        public int PurchasedYears { get; init; }
        public decimal TotalCost { get; init; }
        public decimal MonthlyInstallment { get; init; }
        public decimal RemainingAmount { get; init; }
        public DateTime DeductionStartDate { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
