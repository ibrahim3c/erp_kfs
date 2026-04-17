using HR.Domain;
using HR.Domain.Loans;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateInsurancePurchase
{
    public class CreateInsurancePurchaseCommandHandler : ICommandHandler<CreateInsurancePurchaseCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateInsurancePurchaseCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateInsurancePurchaseCommand request, CancellationToken cancellationToken)
        {
            var result = InsurancePeriodPurchase.Create(
                request.EmployeeId,
                request.InsuranceAuthority,
                request.PurchasedYears,
                request.TotalCost,
                request.MonthlyInstallment,
                request.DeductionStartDate,
                request.ApprovalDecisionFilePath);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.InsurancePurchaseRepository.Add(result.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value.Id);
        }
    }
}
