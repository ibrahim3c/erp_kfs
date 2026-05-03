using HR.Domain;
using HR.Domain.Loans;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.SettleLoan
{
    public class SettleLoanCommandHandler : ICommandHandler<SettleLoanCommand>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public SettleLoanCommandHandler(IHRUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(SettleLoanCommand request, CancellationToken cancellationToken)
        {
           var loan = await unitOfWork.LoanRepository.GetByIdAsync(request.LoanId, cancellationToken);

            if (loan == null)
                return Result.Failure(LoanErrors.NotFoundLoan);

            var result = loan.Settle(DateTime.UtcNow);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
