using HR.Domain;
using HR.Domain.Transfers.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.EndExternalMovement
{
    public class EndExternalMovementHandler : ICommandHandler<EndExternalMovementCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        public EndExternalMovementHandler(IHRUnitOfWork hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public async Task<Result> Handle(EndExternalMovementCommand request, CancellationToken cancellationToken)
        {
            var movement = await _hrUnitOfWork.TranseferRepository.GetExternalMovementByIdAsync(request.MovementId, cancellationToken);
            if (movement == null)
            {
                return Result.Failure(TranseferErrors.NotFoundExternal);
            }

            var result = movement.End();
            if (result.IsFailure) return result;

            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
