using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.GetLoanDetails
{
    public sealed record GetLoanDetailsQueryResponse(
     Guid Id,
     string EmployeeName,
     DateTime StartDate,
     decimal Amount,
     int Months,
     decimal InstallmentAmount,
     decimal RemainingAmount,
     string Reason,
     bool IsCompleted,
     List<LoanInstallmentResponse> Installments);

    public sealed class LoanInstallmentResponse
    {
        public Guid Id { get; init; }
        public int InstallmentNumber { get; init; }
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public bool IsPaid { get; init; }
        public DateTime? PaidAt { get; init; }  
    }
}
