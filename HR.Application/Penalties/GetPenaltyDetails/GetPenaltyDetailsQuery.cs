using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.GetPenaltyDetails
{
    public record GetPenaltyDetailsQuery (Guid Id) : IQuery<GetPenaltyDetailsResponse>;

}
