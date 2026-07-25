using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Command.RenewSecondment
{
    public record RenewSecondmentCommand(Guid SecondmentId, DateTime NewEndDate) : ICommand;
}
