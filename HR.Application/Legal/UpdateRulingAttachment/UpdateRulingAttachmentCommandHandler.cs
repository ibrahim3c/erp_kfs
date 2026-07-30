using HR.Domain;
using HR.Domain.Legal;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Legal.UpdateRulingAttachment
{
    public sealed class UpdateRulingAttachmentCommandHandler
        : ICommandHandler<UpdateRulingAttachmentCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public UpdateRulingAttachmentCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateRulingAttachmentCommand request,
            CancellationToken cancellationToken)
        {
            var ruling = await _unitOfWork.CourtRulingRepository
                .GetByIdAsync(request.RulingId, cancellationToken);

            if (ruling is null)
                return Result.Failure(RulingErrors.NotFound);

            ruling.UpdateAttachment(request.AttachmentPath);
            _unitOfWork.CourtRulingRepository.Update(ruling);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
