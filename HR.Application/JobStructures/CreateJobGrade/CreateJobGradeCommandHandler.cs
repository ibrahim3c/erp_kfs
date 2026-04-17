using HR.Domain;
using HR.Domain.JobStructures;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobGrade
{
    public sealed class CreateJobGradeCommandHandler
         : ICommandHandler<CreateJobGradeCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateJobGradeCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateJobGradeCommand request,
            CancellationToken cancellationToken)
        {
            var result = JobGrade.Create(
                request.Code,
                request.Name,
                request.GradeLevel,
                request.Description,
                request.YearsNo);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.JobStructureRepository.AddJobGrade(result.Value!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value!.Id);
        }
    }
}
