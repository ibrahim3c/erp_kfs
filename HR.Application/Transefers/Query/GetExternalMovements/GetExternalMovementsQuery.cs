using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetExternalMovements
{
    public record GetExternalMovementsQuery : IQuery<List<ExternalMovementListItemDto>>;
}
