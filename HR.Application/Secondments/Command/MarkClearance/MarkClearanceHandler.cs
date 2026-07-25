using HR.Domain;
using HR.Domain.Secondments;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Command.MarkClearance
{
    public class MarkClearanceHandler : ICommandHandler<MarkClearanceCommand>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public MarkClearanceHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(MarkClearanceCommand request, CancellationToken cancellationToken)
        {
            var secondment = await unitOfWork.SecondmentRepository.GetByIdAsync(request.SecondmentId, cancellationToken);
            if (secondment is null)
                return Result.Failure(SecondmentErrors.NotFound);

            secondment.MarkClearanceCompleted();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
