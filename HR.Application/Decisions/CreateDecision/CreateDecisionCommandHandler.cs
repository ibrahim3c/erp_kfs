using HR.Domain.Decisions;
using HR.Domain;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Decisions.CreateDecision
{
    public sealed class CreateDecisionCommandHandler
        : ICommandHandler<CreateDecisionCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateDecisionCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateDecisionCommand request,
            CancellationToken cancellationToken)
        {
            var decisionResult = Decision.Create(
                request.Number,
                request.DecisionTypeId,
                request.DecisionAuthorityId,
                request.DecisionDate,
                request.ValidFrom,
                request.ValidTo,
                request.AffectsEmployee,
                request.AffectsGroup,
                request.IsTemporary,
                request.Subject ?? string.Empty,
                request.Notes ?? string.Empty,
                request.FilePath ?? string.Empty
            );

            if (decisionResult.IsFailure)
                return Result<Guid>.Failure(decisionResult.Error);

            var decision = decisionResult.Value;

            _unitOfWork.DecisionRepository.Add(decision);

            if (request.EmployeeIds is { Length: > 0 })
            {
                foreach (var employeeId in request.EmployeeIds)
                {
                    var employeeDecisionResult = EmployeeDecision.Create(
                        employeeId,
                        decision.Id,
                        request.Subject ?? string.Empty,
                        request.ValidFrom,
                        request.ValidTo,
                        EmployeeDecisionStatus.Active,
                        request.Notes ?? string.Empty
                    );

                    if (employeeDecisionResult.IsFailure)
                        return Result<Guid>.Failure(employeeDecisionResult.Error);

                    _unitOfWork.DecisionRepository.AddEmployeeDecision(employeeDecisionResult.Value);
                    decision.AddEmployeeDecision(employeeDecisionResult.Value);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(decision.Id);
        }
    }
}
