using MediatR;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.AssignLeadershipPosition
{
    public record AssignLeadershipPositionCommand(Guid EmployeeId, Guid LeadershipPositionId) : IRequest<Result<bool>>;
}
