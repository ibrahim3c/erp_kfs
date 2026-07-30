using HR.Domain;
using HR.Domain.Legal;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Legal.ExecuteRuling
{
    public sealed class ExecuteRulingCommandHandler
        : ICommandHandler<ExecuteRulingCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ExecuteRulingCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ExecuteRulingCommand request,
            CancellationToken cancellationToken)
        {
            var ruling = await _unitOfWork.CourtRulingRepository
                .GetByIdAsync(request.RulingId, cancellationToken);

            if (ruling is null)
                return Result.Failure(RulingErrors.NotFound);

            var result = ruling.Execute(request.DecisionId);
            if (result.IsFailure)
                return result;

            _unitOfWork.CourtRulingRepository.Update(ruling);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
