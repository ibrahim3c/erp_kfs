using HR.Domain;
using HR.Domain.Secondments;
using HR.Domain.Secondments.Enums;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Command
{
    public class CreateSecondmentHandler : ICommandHandler<CreateSecondmentCommand, Guid>
    {
        private readonly IHRUnitOfWork unitOfWork;
        private readonly IFileService fileServices;

        public CreateSecondmentHandler(IHRUnitOfWork _unitOfWork,IFileService _fileServices)
        {
            unitOfWork = _unitOfWork;
            fileServices = _fileServices;
        }

        public async Task<Result<Guid>> Handle(CreateSecondmentCommand request, CancellationToken cancellationToken)
        {
            var hasActive = await unitOfWork.SecondmentRepository.CheckItIsActive(request.EmployeeId, SecondmentStatus.Active, cancellationToken);

            if (hasActive)
                return Result<Guid>.Failure(SecondmentErrors.AlreadyActive);

            string? filePath = null;
            if(request.File != null)
            {
                var fileResult = await fileServices.UploadFileAsync(request.File, "Secondments");
                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);
                filePath = fileResult.Value;
            }

            var secondmentResult = Secondment.Create(
                request.EmployeeId, request.Type, request.HostEntityName,
                request.StartDate, request.EndDate, request.SalaryBearer, request.IncentiveBearer, filePath);

            if (secondmentResult.IsFailure)
                return Result<Guid>.Failure(secondmentResult.Error);

            unitOfWork.SecondmentRepository.Add(secondmentResult.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(secondmentResult.Value.Id);
        }
    }
}
