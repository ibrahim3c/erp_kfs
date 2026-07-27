using HR.Domain;
using HR.Domain.Terminations;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Command.CancelTermination
{
    public class CancelTerminationHandler : ICommandHandler<CancelTerminationCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CancelTerminationHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CancelTerminationCommand request, CancellationToken cancellationToken)
        {
            var decision = await _unitOfWork.TerminationRepository.GetByIdAsync(request.TerminationId, cancellationToken);
            if (decision is null)
                return Result.Failure(TerminationErrors.NotFound);

            var result = decision.Cancel(request.CancellationReason);
            if (result.IsFailure) return result;

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(decision.EmployeeId, cancellationToken);
            if (employee is not null)
                employee.Active(); // إعادة تفعيل الموظف

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
