using HR.Domain;
using HR.Domain.Secondments;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Command.RenewSecondment
{
    public class RenewSecondmentHandler : ICommandHandler<RenewSecondmentCommand>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public RenewSecondmentHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(RenewSecondmentCommand request, CancellationToken cancellationToken)
        {
            var secondment = await unitOfWork.SecondmentRepository.GetByIdAsync(request.SecondmentId, cancellationToken);
            if (secondment is null)
                return Result.Failure(SecondmentErrors.NotFound);

            var result = secondment.Renew(request.NewEndDate);
            if (result.IsFailure) return result;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
