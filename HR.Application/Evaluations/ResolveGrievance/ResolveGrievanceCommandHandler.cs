using HR.Domain;
using HR.Domain.Evaluations;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.ResolveGrievance
{
    public sealed class ResolveGrievanceCommandHandler
        : ICommandHandler<ResolveGrievanceCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ResolveGrievanceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            ResolveGrievanceCommand request,
            CancellationToken cancellationToken)
        {
            var grievance = await _unitOfWork.GrievanceRepository
                .GetByIdAsync(request.GrievanceId, cancellationToken);

            if (grievance is null)
                return Result<Guid>.Failure(GrievanceErrors.GrievanceNotFound);

            if (!Enum.TryParse<GrievanceStatus>(request.NewStatus, out var newStatus))
                return Result<Guid>.Failure(new Error(
                    "Grievance.InvalidStatus",
                    "حالة البت غير صحيحة"));

            var resolveResult = grievance.Resolve(
                newStatus,
                request.CommitteeNotes,
                request.ResolutionDate);

            if (resolveResult.IsFailure)
                return Result<Guid>.Failure(resolveResult.Error);

            _unitOfWork.GrievanceRepository.Update(grievance);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(grievance.Id);
        }
    }
}
