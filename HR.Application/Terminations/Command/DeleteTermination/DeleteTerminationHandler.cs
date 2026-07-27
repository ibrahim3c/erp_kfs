using HR.Domain;
using HR.Domain.Terminations;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Command.DeleteTermination
{
    public class DeleteTerminationHandler : ICommandHandler<DeleteTerminationCommand>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public DeleteTerminationHandler(IHRUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteTerminationCommand request, CancellationToken cancellationToken)
        {
            var termination = await unitOfWork.TerminationRepository.GetByIdAsync(request.TerminationId, cancellationToken);
            if (termination == null) 
                return Result.Failure(TerminationErrors.NotFound);

            unitOfWork.TerminationRepository.Delete(termination);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
