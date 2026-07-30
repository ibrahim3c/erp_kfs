using HR.Domain;
using HR.Domain.Funds;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Funds.CreateFundSubscription
{
    public sealed class CreateFundSubscriptionCommandHandler
        : ICommandHandler<CreateFundSubscriptionCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateFundSubscriptionCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateFundSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _unitOfWork.FundRepository
                .GetActiveSubscriptionByEmployeeAsync(request.EmployeeId, request.FundType, cancellationToken);

            if (existing is not null)
                return Result<Guid>.Failure(FundErrors.DuplicateSubscription);

            decimal deductionAmount = request.DeductionAmount;

            if (request.FundType == FundType.Fellowship || request.FundType == FundType.Both)
            {
                var employee = await _unitOfWork.EmployeeRepository
                    .GetByIdAsync(request.EmployeeId, cancellationToken);

                if (employee?.FinancialInfo?.BasicSalary2019 is > 0)
                {
                    deductionAmount = request.FundType == FundType.Both
                        ? employee.FinancialInfo.BasicSalary2019.Value * 0.01m + 50m
                        : employee.FinancialInfo.BasicSalary2019.Value * 0.01m;
                }
            }
            else if (request.FundType == FundType.SocialSolidarity)
            {
                deductionAmount = 50m;
            }

            var subscriptionResult = FundSubscription.Create(
                request.EmployeeId,
                request.SubscriptionDate,
                request.FundType,
                deductionAmount,
                request.BankAgreement,
                request.Notes);

            if (subscriptionResult.IsFailure)
                return Result<Guid>.Failure(subscriptionResult.Error);

            var subscription = subscriptionResult.Value;
            _unitOfWork.FundRepository.AddSubscription(subscription);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(subscription.Id);
        }
    }
}
