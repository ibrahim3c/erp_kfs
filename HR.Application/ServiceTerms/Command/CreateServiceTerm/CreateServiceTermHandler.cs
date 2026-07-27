using HR.Domain;
using HR.Domain.ServiceTerms.Entities;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using Modules.Shared.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.CreateServiceTerm
{
    public class CreateServiceTermHandler : ICommandHandler<CreateServiceTermCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateServiceTermHandler(IHRUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<Result<Guid>> Handle(CreateServiceTermCommand request, CancellationToken cancellationToken)
        {
           string? filePath = null;
            if (request.AttachmentFileName != null)
            {
                var fileResult = await _fileService.UploadFileAsync(request.AttachmentFileName, "ServiceTerms");
                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);
                filePath = fileResult.Value;
            }
            var serviceTerm = ServiceTermRecord.Create(request.EmployeeId, request.PreviousEntityName, request.Type, request.StartDate, request.EndDate, request.CommitteeDecisionNumber, filePath);
            _unitOfWork.ServiceTermRepository.Add(serviceTerm.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(serviceTerm.Value!.Id);
        }
    }
}
