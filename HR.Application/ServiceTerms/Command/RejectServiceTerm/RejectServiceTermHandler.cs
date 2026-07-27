using HR.Domain;
using HR.Domain.ServiceTerms.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.RejectServiceTerm
{
    public class RejectServiceTermHandler : ICommandHandler<RejectServiceTermCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;
        public RejectServiceTermHandler(IHRUnitOfWork hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public async Task<Result> Handle(RejectServiceTermCommand request, CancellationToken cancellationToken)
        {
            var record = await _hrUnitOfWork.ServiceTermRepository.GetByIdAsync(request.ServiceTermId, cancellationToken);
            if (record == null) 
                return Result.Failure(ServiceTermErrors.NotFound);

            var result = record.Reject(request.Reason);
            if (result.IsFailure) return result;

            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
