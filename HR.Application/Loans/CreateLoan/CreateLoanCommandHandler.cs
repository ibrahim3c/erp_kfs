using HR.Domain;
using HR.Domain.Loans;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Loans.CreateLoan
{
    public class CreateLoanCommandHandler : ICommandHandler<CreateLoanCommand, Guid>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public CreateLoanCommandHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            var loanResult = Loan.Create(
                request.EmployeeId,
                request.Amount,
                request.Months,
                request.StartDate,
                request.Reason);

            if (loanResult.IsFailure)
                return Result<Guid>.Failure(loanResult.Error);

            var loan = loanResult.Value;
            // 2. الحفظ في الداتا بيز
            unitOfWork.LoanRepository.Add(loan!);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(loan!.Id);
        }
    }
}
