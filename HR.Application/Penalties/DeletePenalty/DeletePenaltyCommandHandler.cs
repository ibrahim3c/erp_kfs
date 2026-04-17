using HR.Domain;
using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.DeletePenalty
{
    public sealed class DeletePenaltyCommandHandler
        : ICommandHandler<DeletePenaltyCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public DeletePenaltyCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeletePenaltyCommand request,
            CancellationToken cancellationToken)
        {
            var penalty = await _unitOfWork.PenaltyRepository
                .GetByIdAsync(request.PenaltyId, cancellationToken);

            if (penalty is null)
                return Result.Failure(PenaltyErrors.NotFound);

            _unitOfWork.PenaltyRepository.Delete(penalty);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
