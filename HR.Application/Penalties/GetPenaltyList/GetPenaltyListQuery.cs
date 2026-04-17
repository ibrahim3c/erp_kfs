using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.GetPenaltyList
{
    public record GetPenaltyListQuery() : IQuery<List<GetPenaltyListResponse>>;

}
