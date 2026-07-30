using HR.Domain;
using HR.Domain.Attendance;
using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Absence.SettleAbsence
{
    public sealed class SettleAbsenceCommandHandler
        : ICommandHandler<SettleAbsenceCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public SettleAbsenceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            SettleAbsenceCommand request,
            CancellationToken cancellationToken)
        {
            var dateFrom = new DateTime(request.Year, request.Month, 1);
            var dateTo = dateFrom.AddMonths(1).AddDays(-1);

            var allRecords = await _unitOfWork.AttendanceRecordRepository
                .GetByDateRangeAndEmployeeAsync(request.EmployeeId, dateFrom, dateTo, cancellationToken);

            var absentRecords = allRecords?
                .Where(r => r.Status == AttendanceStatus.Absent)
                .ToList();

            if (absentRecords is null || absentRecords.Count == 0)
                return Result<Guid>.Failure(new Error(
                    "Absence.NoUnsettledRecords",
                    "لا توجد سجلات غياب معلقة لهذا الموظف في هذه الفترة"));

            switch (request.ActionType)
            {
                case "DeductBalance":
                    foreach (var record in absentRecords)
                    {
                        record.MarkVacation();
                        record.UpdateNotes(
                            "تحويل غياب إلى أجازة اعتيادية — " + (request.Notes ?? "تسوية يدوية"));
                        _unitOfWork.AttendanceRecordRepository.Update(record);
                    }
                    break;

                case "DeductCasual":
                    foreach (var record in absentRecords)
                    {
                        record.MarkVacation();
                        record.UpdateNotes(
                            "تحويل غياب إلى أجازة عارضة — " + (request.Notes ?? "تسوية يدوية"));
                        _unitOfWork.AttendanceRecordRepository.Update(record);
                    }
                    break;

                case "SalaryPenalty":
                    var employee = await _unitOfWork.EmployeeRepository
                        .GetByIdAsync(request.EmployeeId, cancellationToken);

                    if (employee is null)
                        return Result<Guid>.Failure(new Error(
                            "Absence.EmployeeNotFound",
                            "الموظف غير موجود"));

                    var deductionDays = absentRecords.Count;

                    var penaltyResult = PenaltyRecord.Create(
                        request.EmployeeId,
                        DateTime.Today,
                        PenaltyActionType.Deduct,
                        "غياب بدون أذن — تسوية",
                        deductionDays,
                        DateTime.Today,
                        "تسوية غياب",
                        request.Notes ?? $"خصم {deductionDays} أيام غياب من الراتب",
                        null);

                    if (penaltyResult.IsSuccess)
                    {
                        _unitOfWork.PenaltyRepository.Add(penaltyResult.Value);
                    }

                    foreach (var record in absentRecords)
                    {
                        record.UpdateNotes(
                            "خصم من الراتب — " + (request.Notes ?? "تسوية يدوية"));
                        _unitOfWork.AttendanceRecordRepository.Update(record);
                    }
                    break;

                default:
                    return Result<Guid>.Failure(new Error(
                        "Absence.InvalidActionType",
                        "نوع التسوية غير صحيح"));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(request.EmployeeId);
        }
    }
}
