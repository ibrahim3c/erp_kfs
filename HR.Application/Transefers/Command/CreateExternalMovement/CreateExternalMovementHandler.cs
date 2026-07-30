using HR.Domain;
using HR.Domain.Transfers.Entities;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.CreateExternalMovement
{
    public class CreateExternalMovementHandler : ICommandHandler<CreateExternalMovementCommand,Guid>
    {
        private readonly IFileService _fileService;
        private readonly IHRUnitOfWork _hrUnitOfWork;
        public CreateExternalMovementHandler(IFileService fileService, IHRUnitOfWork hrUnitOfWork)
        {
            _fileService = fileService;
            _hrUnitOfWork = hrUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateExternalMovementCommand request, CancellationToken cancellationToken)
        {

            string? attachmentFilePath = null;
            if (request.AttachmentFileName != null)
            {
                var fileResult = await _fileService.UploadFileAsync(request.AttachmentFileName, "ExternalMovements");
                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);
                attachmentFilePath = fileResult.Value;

            }
            var result = ExternalMovement.Create(
          request.EmployeeId, request.Type, request.Direction, request.OtherEntityName,
          request.StartDate, request.EndDate, request.SalaryBearer, attachmentFilePath);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _hrUnitOfWork.TranseferRepository.AddExternalMovementAsync(result.Value);
            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value.Id);
        }
    }
}
