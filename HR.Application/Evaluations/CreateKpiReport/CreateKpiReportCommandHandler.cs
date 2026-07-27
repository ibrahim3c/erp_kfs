using HR.Domain;
using HR.Domain.Promotions.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.CreateKpiReport
{
    public sealed class CreateKpiReportCommandHandler
        : ICommandHandler<CreateKpiReportCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateKpiReportCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateKpiReportCommand request,
            CancellationToken cancellationToken)
        {
            var totalScore = request.EfficiencyScore + request.DisciplineScore + request.AchievementScore;

            var createResult = KpiReport.Create(
                request.EmployeeId,
                request.Year,
                totalScore,
                request.EfficiencyScore,
                request.DisciplineScore,
                request.AchievementScore,
                request.EvaluatorId,
                request.Status ?? "Draft",
                request.Notes);

            if (createResult.IsFailure)
                return Result<Guid>.Failure(createResult.Error);

            _unitOfWork.KpiReportRepository.Add(createResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(createResult.Value.Id);
        }
    }
}
