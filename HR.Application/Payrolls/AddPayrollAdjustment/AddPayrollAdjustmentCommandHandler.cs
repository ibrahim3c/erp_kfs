using HR.Domain;
using HR.Domain.Payrolls;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.AddPayrollAdjustment
{
    public sealed class AddPayrollAdjustmentCommandHandler
         : ICommandHandler<AddPayrollAdjustmentCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public AddPayrollAdjustmentCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddPayrollAdjustmentCommand request,
            CancellationToken cancellationToken)
        {
            var entry = await _unitOfWork.PayrollRepository
                .GetPayrollEntryByIdAsync(request.EntryId, cancellationToken);

            if (entry is null)
                return Result.Failure(PayrollErrors.EntryNotFound);

            var result = entry.AddAdjustment(request.Type, request.Amount, request.Reason);
            if (result.IsFailure) return result;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
