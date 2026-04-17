using HR.Application.Loans.GetInsurancePurchaseList;
using HR.Application.Loans.GetLoanList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans
{
    public class LoanRequestsViewModel
    {
                public List<GetLoanListResponse> Loans { get; init; } = new();
                public List<GetInsurancePurchaseListResponse> InsurancePurchases { get; init; } = new();
    }
}
