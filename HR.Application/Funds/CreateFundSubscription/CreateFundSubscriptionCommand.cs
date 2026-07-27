using HR.Domain.Funds;
using Modules.Shared.Application.Messaging;

namespace HR.Application.Funds.CreateFundSubscription
{
    public record CreateFundSubscriptionCommand(
        Guid EmployeeId,
        DateTime SubscriptionDate,
        FundType FundType,
        decimal DeductionAmount,
        bool BankAgreement,
        string? Notes
    ) : ICommand<Guid>;
}
