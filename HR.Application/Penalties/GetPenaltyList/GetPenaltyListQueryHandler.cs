using HR.Domain;
using HR.Domain.Penalties;
using MediatR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.GetPenaltyList
{
    public class GetPenaltyListQueryHandler : IQueryHandler<GetPenaltyListQuery, List<GetPenaltyListResponse>>
    {
        private readonly IHRUnitOfWork unitOfWork;

        public GetPenaltyListQueryHandler(IHRUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<List<GetPenaltyListResponse>>> Handle(GetPenaltyListQuery request, CancellationToken cancellationToken)
        {
            var penalties = unitOfWork.PenaltyRepository.GetAllAsync(cancellationToken);

            if (penalties is null)
                return Result<List<GetPenaltyListResponse>>.Failure(PenaltyErrors.AllNotFound);

            var result = penalties.Result.Select(p => new GetPenaltyListResponse
            {
                Id = p.Id,
                ActionType = p.ActionType,
                DecisionReference = p.DecisionReference,
                DeductionDays = p.DeductionDays,
                EmployeeName = p.Employee.Name,
                ExecutionMonth = p.ExecutionMonth,
                PenaltyType = p.PenaltyType,
                ViolationDate = p.ViolationDate,
                AttachmentPathath = p.AttachmentPath
            }).ToList();
            
            return Result<List<GetPenaltyListResponse>>.Success(result);
        }
    }
}
