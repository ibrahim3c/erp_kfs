using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Loans;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.RejectInsurancePurchase
{
    public class RejectInsurancePurchaseCommandHandler : ICommandHandler<RejectInsurancePurchaseCommand>
    {
        private readonly IHRUnitOfWork hRUnitOfWork;

        public RejectInsurancePurchaseCommandHandler(IHRUnitOfWork hRUnitOfWork)
        {
            this.hRUnitOfWork = hRUnitOfWork;
        }
        public async Task<Result> Handle(RejectInsurancePurchaseCommand request, CancellationToken cancellationToken)
        {

            var purchase = await hRUnitOfWork.InsurancePurchaseRepository
                .GetByIdAsync(request.PurchaseId, cancellationToken);

            if (purchase is null)
                return Result.Failure(InsurancePurchaseErrors.NotFound);

            var approveResult = purchase.Reject();
            if (approveResult.IsFailure)
                return approveResult;

            await hRUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
