using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Transfers.Entities
{
    public interface ITranseferRepository
    {
        void AddInternalTransferAsync(InternalTransfer internalTransfer);
        void AddExternalMovementAsync(ExternalMovement externalMovement);
        void DeleteInternalTransferAsync(InternalTransfer internalTransfer);
        void DeleteExternalMovementAsync(ExternalMovement externalMovement);
        Task<InternalTransfer?> GetInternalTransferByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ExternalMovement?> GetExternalMovementByIdAsync(Guid id, CancellationToken cancellationToken = default);
   
    }
}
