using HR.Domain;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.CreateLateEntry
{
    public sealed class CreateLateEntryCommandHandler
        : ICommandHandler<CreateLateEntryCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateLateEntryCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateLateEntryCommand request,
            CancellationToken cancellationToken)
        {
            var result = LateEntry.Create(
                request.EmployeeId,
                request.Date,
                request.ActualArrivalTime,
                request.Notes);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.LateEntryRepository.Add(result.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // ── تحقق من تجاوز الحد الشهري → تحويل لجزاء تلقائي ──
            var monthlyMinutes = await _unitOfWork.LateEntryRepository
                .GetMonthlyLateMinutesAsync(
                    request.EmployeeId,
                    request.Date.Month,
                    request.Date.Year,
                    cancellationToken);

            if (monthlyMinutes >= LateEntry.MinutesPerPenaltyDay)
            {
                // نقل كل التأخيرات غير المحولة → Penalty
                var pendingEntries = await _unitOfWork.LateEntryRepository
                    .GetPendingTransferAsync(
                        request.EmployeeId,
                        request.Date.Month,
                        request.Date.Year,
                        cancellationToken);

                int penaltyDays = monthlyMinutes / LateEntry.MinutesPerPenaltyDay;

                var penalty = PenaltyRecord.Create(
                    employeeId: request.EmployeeId,
                    violationDate: request.Date,
                    actionType: PenaltyActionType.Deduct,
                    penaltyType: "تأخير متكرر",
                    deductionDays: penaltyDays,
                    executionMonth: new DateTime(request.Date.Year, request.Date.Month, 1),
                    decisionReference: $"تأخيرات {request.Date:MM/yyyy}",
                    notes: $"تجاوز {monthlyMinutes} دقيقة تأخير خلال الشهر",
                    attachmentPath: string.Empty);

                if (penalty.IsSuccess)
                {
                    _unitOfWork.PenaltyRepository.Add(penalty.Value!);

                    foreach (var entry in pendingEntries)
                        entry.MarkAsTransferredToPenalty();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result<Guid>.Success(result.Value!.Id);
        }
    }
}
