using HR.Domain;
using HR.Domain.Permissions;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.DeletePermission
{
    public class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public DeletePermissionCommandHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<Result> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await unitOfWork.PermissionRepository.GetByIdAsync(request.PermissionId);

            if (permission == null)
                return Result.Failure(AttendanceErrors.NotFound);

            unitOfWork.PermissionRepository.Delete(permission);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
