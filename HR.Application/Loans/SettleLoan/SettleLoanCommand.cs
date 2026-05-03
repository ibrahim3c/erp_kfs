using Modules.Shared.Application.Messaging;


namespace HR.Application.Loans.SettleLoan
{
    public record SettleLoanCommand(Guid LoanId) : ICommand;

}
