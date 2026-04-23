using HR.Domain;
using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.CreatePermission
{
    public sealed class CreatePermissionCommandHandler
         : ICommandHandler<CreatePermissionCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreatePermissionCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            // التحقق من الحد الشهري — مرتين أو 4 ساعات
            if (request.PermissionType == PermissionType.Personal)
            {
                var monthlyStats = await _unitOfWork.PermissionRepository
                    .GetMonthlyStatsAsync(
                        request.EmployeeId,
                        request.Date.Month,
                        request.Date.Year,
                        cancellationToken);

                if (monthlyStats.Count >= 2)
                    return Result<Guid>.Failure(AttendanceErrors.MonthlyCountExceeded);

                var newDuration = (int)(request.ToTime - request.FromTime).TotalMinutes;
                if (monthlyStats.TotalMinutes + newDuration > 240)
                    return Result<Guid>.Failure(AttendanceErrors.MonthlyHoursExceeded);
            }

            var result = PermissionRequest.Create(
                request.EmployeeId,
                request.PermissionType,
                request.Date,
                request.FromTime,
                request.ToTime,
                request.Notes);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.PermissionRepository.Add(result.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value!.Id);
        }
    }
}
