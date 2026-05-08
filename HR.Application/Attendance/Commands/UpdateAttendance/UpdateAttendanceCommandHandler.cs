using HR.Domain;
using HR.Domain.Attendance;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Commands.UpdateAttendance
{
    public sealed class UpdateAttendanceCommandHandler
        : ICommandHandler<UpdateAttendanceCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public UpdateAttendanceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            UpdateAttendanceCommand request,
            CancellationToken cancellationToken)
        {
            var record = await _unitOfWork.AttendanceRecordRepository
                .GetByIdAsync(request.Id, cancellationToken);

            if (record is null)
                return Result<Guid>.Failure(AttendanceErrors.NotFound);

            if (request.CheckIn.HasValue)
            {
                var result = record.RecordCheckIn(request.CheckIn.Value);
                if (result.IsFailure)
                    return Result<Guid>.Failure(result.Error);
            }

            if (request.CheckOut.HasValue)
            {
                var result = record.RecordCheckOut(request.CheckOut.Value);
                if (result.IsFailure)
                    return Result<Guid>.Failure(result.Error);
            }

            record.UpdateNotes(request.Notes);

            _unitOfWork.AttendanceRecordRepository.Update(record);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(record.Id);
        }
    }
}
