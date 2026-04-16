using HR.Domain;
using HR.Domain.Loans;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.ApproveInsurancePurchase
{
    public class ApproveInsurancePurchaseCommandHandler
         : ICommandHandler<ApproveInsurancePurchaseCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ApproveInsurancePurchaseCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ApproveInsurancePurchaseCommand request,
            CancellationToken cancellationToken)
        {
            var purchase = await _unitOfWork.InsurancePurchaseRepository
                .GetByIdAsync(request.PurchaseId, cancellationToken);

            if (purchase is null)
                return Result.Failure(InsurancePurchaseErrors.NotFound);

            var approveResult = purchase.Approve();
            if (approveResult.IsFailure)
                return approveResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
