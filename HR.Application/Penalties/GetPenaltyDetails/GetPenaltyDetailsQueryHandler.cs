using HR.Domain;
using HR.Domain.Penalties;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.GetPenaltyDetails
{
    public class GetPenaltyDetailsQueryHandler : IQueryHandler<GetPenaltyDetailsQuery, GetPenaltyDetailsResponse>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public GetPenaltyDetailsQueryHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<Result<GetPenaltyDetailsResponse>> Handle(GetPenaltyDetailsQuery request, CancellationToken cancellationToken)
        {
            var penalty = await unitOfWork.PenaltyRepository.GetByIdAsync(request.Id);
            if (penalty == null)
                return Result<GetPenaltyDetailsResponse>.Failure(PenaltyErrors.NotFound);

            var response = new GetPenaltyDetailsResponse
            {
                Id = penalty.Id,
                EmployeeName = penalty.Employee.Name,
                ActionType = penalty.ActionType,
                ViolationDate = penalty.ViolationDate,
                PenaltyType = penalty.PenaltyType,
                DeductionDays = penalty.DeductionDays,
                ExecutionMonth = penalty.ExecutionMonth,
                DecisionReference = penalty.DecisionReference,
                AttachmentPathath = penalty.AttachmentPath,
                Notes = penalty.Notes
            };

            return Result<GetPenaltyDetailsResponse>.Success(response);
        }
    }
}
