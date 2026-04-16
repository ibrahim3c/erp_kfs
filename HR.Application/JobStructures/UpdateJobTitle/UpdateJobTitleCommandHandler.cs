using HR.Domain;
using HR.Domain.JobStructures;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.UpdateJobTitle
{
    public sealed class UpdateJobTitleCommandHandler
        : ICommandHandler<UpdateJobTitleCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public UpdateJobTitleCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateJobTitleCommand request,
            CancellationToken cancellationToken)
        {
            var jobTitle = await _unitOfWork.JobStructureRepository
                .GetJobTitleByIdAsync(request.Id, cancellationToken);

            if (jobTitle is null)
                return Result.Failure(JobStructureErrors.NotFound);

            var result = jobTitle.UpdateDetails(request.Code, request.Name, request.Description);
            if (result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
