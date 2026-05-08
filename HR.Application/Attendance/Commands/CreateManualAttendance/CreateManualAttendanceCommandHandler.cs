using HR.Domain;
using HR.Domain.Attendance;
using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Commands.CreateManualAttendance
{
    public sealed class CreateManualAttendanceCommandHandler
        : ICommandHandler<CreateManualAttendanceCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateManualAttendanceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateManualAttendanceCommand request,
            CancellationToken cancellationToken)
        {
            var isCheckIn = request.MovementType == MovementType.CheckIn;

            var existing = await _unitOfWork.AttendanceRecordRepository
                .GetByEmployeeAndDateAsync(request.EmployeeId, request.Date, cancellationToken);

            if (existing is not null)
            {
                // Record already exists — update check-in or check-out
                var updateResult = isCheckIn
                    ? existing.RecordCheckIn(request.Time)
                    : existing.RecordCheckOut(request.Time);

                if (updateResult.IsFailure)
                    return Result<Guid>.Failure(updateResult.Error);

                if (!string.IsNullOrWhiteSpace(request.Notes))
                    existing.UpdateNotes(request.Notes);


                if( isCheckIn && existing.Status == AttendanceStatus.Late)
                {
                    var lateEntry = LateEntry.Create(request.EmployeeId, request.Date, request.Time, request.Notes);
                    if (lateEntry.IsSuccess)
                        return Result<Guid>.Failure(lateEntry.Error);
                   _unitOfWork.LateEntryRepository.Add(lateEntry.Value);
                    existing.LinkLateEntry(lateEntry.Value.Id);
                }

                _unitOfWork.AttendanceRecordRepository.Update(existing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(existing.Id);
            }

            // New record
            //var status = isCheckIn
            //    ? AttendanceStatus.Present
            //    : AttendanceStatus.Present;

            var createResult = AttendanceRecord.Create(
                request.EmployeeId,
                request.Date,
                //isCheckIn ? request.Time : null,
                //isCheckIn ? null : request.Time,
                null,
                null,
                AttendanceStatus.Present,
                request.Notes);

            if (createResult.IsFailure)
                return Result<Guid>.Failure(createResult.Error);
            if(isCheckIn)
                createResult.Value!.RecordCheckIn(request.Time);
            else
                createResult.Value!.RecordCheckOut(request.Time);

            _unitOfWork.AttendanceRecordRepository.Add(createResult.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(createResult.Value!.Id);
        }
    }
}
