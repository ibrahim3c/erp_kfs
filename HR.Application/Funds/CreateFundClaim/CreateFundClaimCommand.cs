using HR.Domain.Funds;
using Modules.Shared.Application.Messaging;

namespace HR.Application.Funds.CreateFundClaim
{
    public record CreateFundClaimCommand(
        Guid EmployeeId,
        FundClaimType ClaimType,
        DateTime EventDate,
        decimal? Amount,
        string? AttachmentPath
    ) : ICommand<Guid>;
}
