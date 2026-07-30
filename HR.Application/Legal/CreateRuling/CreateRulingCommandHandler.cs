using HR.Domain;
using HR.Domain.Legal;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Legal.CreateRuling
{
    public sealed class CreateRulingCommandHandler
        : ICommandHandler<CreateRulingCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateRulingCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateRulingCommand request,
            CancellationToken cancellationToken)
        {
            var rulingResult = CourtRuling.Create(
                request.CaseNumber,
                request.Year,
                request.EmployeeId,
                request.EmployeeName,
                request.Summary,
                request.ExecutionType,
                request.AttachmentPath);

            if (rulingResult.IsFailure)
                return Result<Guid>.Failure(rulingResult.Error);

            var ruling = rulingResult.Value;
            _unitOfWork.CourtRulingRepository.Add(ruling);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(ruling.Id);
        }
    }
}
