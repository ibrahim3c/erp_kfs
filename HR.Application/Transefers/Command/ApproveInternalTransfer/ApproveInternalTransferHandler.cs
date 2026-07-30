using HR.Domain;
using HR.Domain.Employees;
using HR.Domain.Transfers.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.ApproveInternalTransfer
{
    public class ApproveInternalTransferHandler : ICommandHandler<ApproveInternalTransferCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        public ApproveInternalTransferHandler(IHRUnitOfWork hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public async Task<Result> Handle(ApproveInternalTransferCommand request, CancellationToken cancellationToken)
        {
            

            var transfer = await _hrUnitOfWork.TranseferRepository.GetInternalTransferByIdAsync(request.TransferId, cancellationToken);
            if(transfer is null)
                return Result.Failure(TranseferErrors.NotFoundInternal);

            var employee = await _hrUnitOfWork.EmployeeRepository.GetByIdAsync(transfer.EmployeeId, cancellationToken);
            if (employee is null)
                return Result.Failure(EmployeeErrors.NotFound);

            var result = transfer.Approve();
            if (result.IsFailure) return result;

           employee.UpdateJobTitleAndOrgUnit(transfer.NewJobTitleId, transfer.ToDepartmentId);


            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
