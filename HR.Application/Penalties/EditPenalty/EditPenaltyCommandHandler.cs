using CollegeControlSystem.Domain.Abstractions;
using HR.Domain;
using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.EditPenalty
{
    public class EditPenaltyCommandHandler : ICommandHandler<EditPenaltyCommand, Guid>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public EditPenaltyCommandHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<Result<Guid>> Handle(EditPenaltyCommand request, CancellationToken cancellationToken)
        {
            var penalty = await unitOfWork.PenaltyRepository.GetByIdAsync(request.PenaltyId, cancellationToken);
            if (penalty == null)
                return Result<Guid>.Failure(PenaltyErrors.NotFound);

            var updateResult = penalty.Update(
                request.ViolationDate,
                request.ActionType,
                request.PenaltyType,
                request.DeductionDays,
                request.ExecutionMonth,
                request.DecisionReference ?? string.Empty,
                request.Notes ?? string.Empty,
                request.AttachmentPath ?? string.Empty
            );

            if (updateResult.IsFailure)
                return Result<Guid>.Failure(updateResult.Error);

            // 3. تحديث السجل في الـ Repository وحفظ التغييرات (Save)
            unitOfWork.PenaltyRepository.Update(penalty);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(penalty.Id);
        }
    }
}
