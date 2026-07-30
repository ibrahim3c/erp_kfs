using HR.Domain;
using HR.Domain.Transfers.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.RenewExternalMovement
{
    public class RenewExternalMovementHandler : ICommandHandler<RenewExternalMovementCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        public RenewExternalMovementHandler(IHRUnitOfWork hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public async Task<Result> Handle(RenewExternalMovementCommand request, CancellationToken cancellationToken)
        {
            var movement = await _hrUnitOfWork.TranseferRepository.GetExternalMovementByIdAsync(request.MovementId, cancellationToken);
            if(movement == null)
            {
                return Result.Failure(TranseferErrors.NotFoundExternal);
            }

            var result = movement.Renew(request.NewEndDate);
            if (result.IsFailure) return result;

            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
