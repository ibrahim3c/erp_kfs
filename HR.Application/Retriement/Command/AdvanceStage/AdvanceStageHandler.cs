using HR.Domain;
using HR.Domain.Retirement.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Command.AdvanceStage
{
    public class AdvanceStageHandler : ICommandHandler<AdvanceStageCommand>
    {
        private readonly IHRUnitOfWork hrUnitOfWork;

        public AdvanceStageHandler(IHRUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork; 
        }
        public async Task<Result> Handle(AdvanceStageCommand request, CancellationToken cancellationToken)
        {
            var file = await hrUnitOfWork.RetriementRepository.GetByIdAsync(request.RetirementFileId, cancellationToken);
            if (file is null)
                return Result.Failure(RetirementErrors.NotFound);

            var result = file.AdvanceStage(request.NextStage);
            if (result.IsFailure) return result;

            await hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
