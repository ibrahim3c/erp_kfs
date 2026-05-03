using Modules.Shared.Application.Messaging;


namespace HR.Application.Loans.RejectInsurancePurchase
{
    public record RejectInsurancePurchaseCommand(Guid PurchaseId) : ICommand;
    
}
