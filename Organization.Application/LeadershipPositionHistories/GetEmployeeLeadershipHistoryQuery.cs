using MediatR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.LeadershipPositionHistories
{
    public sealed record GetEmployeeLeadershipHistoryQuery(Guid EmployeeId)
    : IQuery<List<EmployeeLeadershipHistoryResponse>>;
}
