using HR.Domain;
using HR.Domain.Transfers.Entities;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using Modules.Shared.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Command.CreateInternalTransfer
{
    public class CreateInternalTransferHandler : ICommandHandler<CreateInternalTransferCommand, Guid>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        private readonly IFileService _fileService;
        public CreateInternalTransferHandler(IHRUnitOfWork hrUnitOfWork,IFileService fileService)
        {
            _hrUnitOfWork = hrUnitOfWork;
            _fileService = fileService;
        }
        public async Task<Result<Guid>> Handle(CreateInternalTransferCommand request, CancellationToken cancellationToken)
        {
            string? attachmentFilePath = null;
            if (request.AttachmentFileName != null)
            {
                var fileResult = await _fileService.UploadFileAsync(request.AttachmentFileName, "InternalTransfers");
                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);
                attachmentFilePath = fileResult.Value;

            }

            var result = InternalTransfer.Create(
            request.EmployeeId, request.FromDepartmentId, request.ToDepartmentId,
            request.Reason, request.ExecutionDate, request.NewJobTitleId, attachmentFilePath);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _hrUnitOfWork.TranseferRepository.AddInternalTransferAsync(result.Value);
            await _hrUnitOfWork.SaveChangesAsync(cancellationToken); 

            return Result<Guid>.Success(result.Value.Id);
        }
    }
}
