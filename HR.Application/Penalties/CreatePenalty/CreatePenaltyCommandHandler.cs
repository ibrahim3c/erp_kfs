using HR.Domain;
using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.CreatePenalty
{
    public sealed class CreatePenaltyCommandHandler
         : ICommandHandler<CreatePenaltyCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreatePenaltyCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreatePenaltyCommand request,
            CancellationToken cancellationToken)
        {       
            var result = PenaltyRecord.Create(
                request.EmployeeId,
                request.ViolationDate,
                request.ActionType,
                request.PenaltyType,
                request.DeductionDays,
                request.ExecutionMonth,
                request.DecisionReference ?? string.Empty, 
                request.Notes ?? string.Empty,             
                request.AttachmentPath ?? string.Empty    
            );

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            _unitOfWork.PenaltyRepository.Add(result!.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(result.Value.Id);
        }
    }
}

