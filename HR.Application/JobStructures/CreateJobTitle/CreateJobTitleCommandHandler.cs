using HR.Domain;
using HR.Domain.JobStructures;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobTitle
{
    public sealed class CreateJobTitleCommandHandler
       : ICommandHandler<CreateJobTitleCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateJobTitleCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateJobTitleCommand request,
            CancellationToken cancellationToken)
        {
            var result = JobTitle.Create(
                request.FunctionalGroupId,
                request.Code,
                request.Name,
                request.Description);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.JobStructureRepository.AddJobTitle(result.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value!.Id);
        }
    }
}
