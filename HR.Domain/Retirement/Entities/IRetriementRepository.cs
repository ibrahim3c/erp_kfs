using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Entities
{
    public interface IRetriementRepository
    {
        void Add(RetirementFile retirement);
        Task<RetirementFile?> GetByIdAsync(Guid retirementId, CancellationToken cancellationToken = default);

    }
}
