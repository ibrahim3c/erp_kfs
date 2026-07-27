using HR.Domain;
using HR.Domain.Employees;
using HR.Domain.Terminations;
using HR.Domain.Terminations.Enums;
using Modules.Shared.Application.IService;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using Modules.Shared.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Command.CreateTermination
{
    public class CreateTerminationHandler : ICommandHandler<CreateTerminationCommand, Guid>
    {
        private readonly IHRUnitOfWork unitOfWork;
        private readonly IFileService fileService;

        public CreateTerminationHandler(IHRUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result<Guid>> Handle(CreateTerminationCommand request, CancellationToken cancellationToken)
        {
            var alreadyExists = await unitOfWork.TerminationRepository.AnyAsync(request.EmployeeId, TerminationStatus.Executed, cancellationToken);
            if (alreadyExists)
                return Result<Guid>.Failure(TerminationErrors.AlreadyExecuted); 

            var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee == null)
                return Result<Guid>.Failure(EmployeeErrors.NotFound);

            string? attachmentFilePath = null;
            if(request.AttachmentFile != null)
            {
                var fileResult = await fileService.UploadFileAsync(request.AttachmentFile, "Terminations");
                if (fileResult.IsFailure)
                    return Result<Guid>.Failure(fileResult.Error);
                attachmentFilePath = fileResult.Value;
            }

            var decisionResult = TerminationDecision.Create(
           request.EmployeeId, request.DecisionNumber, request.Reason,
           request.DecisionDate, request.LastWorkingDay, request.LegalBasis, attachmentFilePath);

            if (decisionResult.IsFailure)
                return Result<Guid>.Failure(decisionResult.Error);

            // الأثر الحقيقي: إيقاف الموظف فعليًا في النظام
            employee.Deactivate();

            unitOfWork.TerminationRepository.Add(decisionResult.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(decisionResult.Value.Id);

        }
    }
}
