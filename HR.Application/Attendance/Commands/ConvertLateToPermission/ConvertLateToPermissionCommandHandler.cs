using HR.Domain;
using HR.Domain.Attendance;
using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Attendance.Commands.ConvertLateToPermission
{
    public sealed class ConvertLateToPermissionCommandHandler
        : ICommandHandler<ConvertLateToPermissionCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ConvertLateToPermissionCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            ConvertLateToPermissionCommand request,
            CancellationToken cancellationToken)
        {
            var record = await _unitOfWork.AttendanceRecordRepository
                .GetByIdAsync(request.AttendanceRecordId, cancellationToken);

            if (record is null)
                return Result<Guid>.Failure(HR.Domain.Attendance.AttendanceErrors.NotFound);

            if (record.Status != AttendanceStatus.Late)
                return Result<Guid>.Failure(new Error(
                    "Attendance.NotLateStatus",
                    "لا يمكن تحويل هذا السجل إلى إذن لأن الحالة ليست تأخير"));

            var permissionResult = PermissionRequest.Create(
                record.EmployeeId,
                request.PermissionType,
                request.Date,
                request.FromTime,
                request.ToTime,
                request.Notes ?? string.Empty);

            if (permissionResult.IsFailure)
                return Result<Guid>.Failure(permissionResult.Error);

            _unitOfWork.PermissionRepository.Add(permissionResult.Value!);

            record.LinkPermission(permissionResult.Value!.Id);
            record.UpdateNotes(request.Notes ?? string.Empty);

            _unitOfWork.AttendanceRecordRepository.Update(record);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(record.Id);
        }
    }
}
