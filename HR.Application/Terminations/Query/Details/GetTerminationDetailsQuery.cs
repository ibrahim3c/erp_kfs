using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.Details
{
    public record GetTerminationDetailsQuery(Guid TerminationId) : IQuery<TerminationDetailsDto>;
  
}
