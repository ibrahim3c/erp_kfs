using HR.Domain;
using HR.Domain.Retirement.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Command.UpdateFinancialData
{
    public class UpdateFinancialDataHandler : ICommandHandler<UpdateFinancialDataCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public UpdateFinancialDataHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateFinancialDataCommand request, CancellationToken cancellationToken)
        {
            var file = await _unitOfWork.RetriementRepository.GetByIdAsync(request.RetirementFileId, cancellationToken);
            if (file is null)
                return Result.Failure(RetirementErrors.NotFound);

            foreach (var (year, amount) in request.YearAmounts)
                file.AddOrUpdateSalaryYear(year, amount);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
