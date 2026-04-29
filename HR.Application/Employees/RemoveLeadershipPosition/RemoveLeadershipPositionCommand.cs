using MediatR;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.RemoveLeadershipPosition
{
    public record RemoveLeadershipPositionCommand(Guid EmployeeId) : IRequest<Result<bool>>;
}
