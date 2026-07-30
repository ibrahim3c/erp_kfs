using HR.Domain.Terminations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Terminations
{
    public interface ITerminationRepository
    {
        Task<TerminationDecision?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TerminationDecision?> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Guid employeeId,TerminationStatus status,CancellationToken cancellationToken = default);
        void Add(TerminationDecision terminationDecision);
        void Delete(TerminationDecision terminationDecision);
    }
}
