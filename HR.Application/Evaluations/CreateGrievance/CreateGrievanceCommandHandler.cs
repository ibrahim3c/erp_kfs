using HR.Domain;
using HR.Domain.Evaluations;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.CreateGrievance
{
    public sealed class CreateGrievanceCommandHandler
        : ICommandHandler<CreateGrievanceCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateGrievanceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateGrievanceCommand request,
            CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<GrievanceType>(request.GrievanceType, out var grievanceType))
                return Result<Guid>.Failure(GrievanceErrors.GrievanceTypeRequired);

            var createResult = Grievance.Create(
                request.EmployeeId,
                grievanceType,
                request.ComplainedDecisionNumber,
                request.ComplainedDecisionDate,
                request.SubmissionDate,
                request.Reasons,
                request.AttachmentPath);

            if (createResult.IsFailure)
                return Result<Guid>.Failure(createResult.Error);

            _unitOfWork.GrievanceRepository.Add(createResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(createResult.Value.Id);
        }
    }
}
