using HR.Domain;
using HR.Domain.Payrolls;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.LockPayrollCycle
{

    public sealed class LockPayrollCycleCommandHandler
        : ICommandHandler<LockPayrollCycleCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public LockPayrollCycleCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            LockPayrollCycleCommand request,
            CancellationToken cancellationToken)
        {
            var cycle = await _unitOfWork.PayrollRepository
                .GetPayrollCycleByIdAsync(request.CycleId, cancellationToken);

            if (cycle is null)
                return Result.Failure(PayrollErrors.CycleNotFound);

            var result = cycle.Lock();
            if (result.IsFailure) return result;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
