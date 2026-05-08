using HR.Domain;
using HR.Domain.Attendance;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Commands.ConvertAbsenceToVacation
{
    public sealed class ConvertAbsenceToVacationCommandHandler
        : ICommandHandler<ConvertAbsenceToVacationCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ConvertAbsenceToVacationCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            ConvertAbsenceToVacationCommand request,
            CancellationToken cancellationToken)
        {
            var record = await _unitOfWork.AttendanceRecordRepository
                .GetByIdAsync(request.AttendanceRecordId, cancellationToken);

            if (record is null)
                return Result<Guid>.Failure(AttendanceErrors.NotFound);

            if (record.Status != AttendanceStatus.Absent)
                return Result<Guid>.Failure(new Error(
                    "Attendance.NotAbsentStatus",
                    "لا يمكن تحويل هذا السجل إلى أجازة لأن الحالة ليست غياب"));

            record.MarkVacation();

            var notes = request.VacationType switch
            {
                "sick" => "تحويل غياب إلى أجازة مرضية",
                "regular" => "تحويل غياب إلى أجازة اعتيادية",
                _ => "تحويل غياب إلى أجازة"
            };

            if (!string.IsNullOrWhiteSpace(request.Notes))
                notes += " — " + request.Notes;

            record.UpdateNotes(notes);

            _unitOfWork.AttendanceRecordRepository.Update(record);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(record.Id);
        }
    }
}
