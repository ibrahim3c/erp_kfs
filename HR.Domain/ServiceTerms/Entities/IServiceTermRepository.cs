using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.ServiceTerms.Entities
{
    public interface IServiceTermRepository
    {
        void Add(ServiceTermRecord serviceTerm);
        void Delete(ServiceTermRecord serviceTerm);
        Task<ServiceTermRecord?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
    }
}
