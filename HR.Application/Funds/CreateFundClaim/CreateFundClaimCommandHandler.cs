using HR.Domain;
using HR.Domain.Funds;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Funds.CreateFundClaim
{
    public sealed class CreateFundClaimCommandHandler
        : ICommandHandler<CreateFundClaimCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateFundClaimCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateFundClaimCommand request,
            CancellationToken cancellationToken)
        {
            var claimResult = FundClaim.Create(
                request.EmployeeId,
                request.ClaimType,
                request.EventDate,
                request.Amount,
                request.AttachmentPath);

            if (claimResult.IsFailure)
                return Result<Guid>.Failure(claimResult.Error);

            var claim = claimResult.Value;
            _unitOfWork.FundRepository.AddClaim(claim);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(claim.Id);
        }
    }
}
