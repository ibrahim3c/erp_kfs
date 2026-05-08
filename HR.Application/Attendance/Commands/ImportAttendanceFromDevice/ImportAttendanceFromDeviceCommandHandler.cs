using HR.Domain;
using HR.Domain.Attendance;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Commands.ImportAttendanceFromDevice
{
    public sealed class ImportAttendanceFromDeviceCommandHandler
        : ICommandHandler<ImportAttendanceFromDeviceCommand, int>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ImportAttendanceFromDeviceCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
            ImportAttendanceFromDeviceCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Records is null || request.Records.Count == 0)
                return Result<int>.Failure(new Error(
                    "Attendance.NoRecords",
                    "لا توجد سجلات للاستيراد"));

            var imported = 0;

            foreach (var deviceRecord in request.Records)
            {
                var isCheckIn = deviceRecord.Direction.Equals("in", StringComparison.OrdinalIgnoreCase);

                var existing = await _unitOfWork.AttendanceRecordRepository
                    .GetByEmployeeAndDateAsync(deviceRecord.EmployeeId, deviceRecord.Date, cancellationToken);

                if (existing is not null)
                {
                    var updateResult = isCheckIn
                        ? existing.RecordCheckIn(deviceRecord.Time)
                        : existing.RecordCheckOut(deviceRecord.Time);

                    if (updateResult.IsSuccess)
                    {
                        _unitOfWork.AttendanceRecordRepository.Update(existing);
                        imported++;
                    }
                }
                else
                {
                    var createResult = AttendanceRecord.Create(
                        deviceRecord.EmployeeId,
                        deviceRecord.Date,
                        isCheckIn ? deviceRecord.Time : null,
                        isCheckIn ? null : deviceRecord.Time,
                        AttendanceStatus.Present,
                        "مستورد من جهاز البصمة");

                    if (createResult.IsSuccess)
                    {
                        _unitOfWork.AttendanceRecordRepository.Add(createResult.Value!);
                        imported++;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(imported);
        }
    }
}
