using System;
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
       bool IsCompleted);
}
