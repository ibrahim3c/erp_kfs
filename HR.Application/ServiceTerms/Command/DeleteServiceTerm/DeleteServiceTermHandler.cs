using HR.Domain;
using HR.Domain.ServiceTerms.Entities;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.DeleteServiceTerm
{
    public class DeleteServiceTermHandler : ICommandHandler<DeleteServiceTermCommand>
    {
        private readonly IHRUnitOfWork _hrUnitOfWork;

        public DeleteServiceTermHandler(IHRUnitOfWork hrUnitOfWork)
        {
            _hrUnitOfWork = hrUnitOfWork;
        }
        public async Task<Result> Handle(DeleteServiceTermCommand request, CancellationToken cancellationToken)
        {
            var serviceTerm = await _hrUnitOfWork.ServiceTermRepository.GetByIdAsync(request.ServiceTermId, cancellationToken);
            if (serviceTerm == null)
            {
                return Result.Failure(ServiceTermErrors.NotFound);
            }
            _hrUnitOfWork.ServiceTermRepository.Delete(serviceTerm);
            await _hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
