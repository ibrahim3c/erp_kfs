using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.ServiceTerms.Command.DeleteServiceTerm
{
    public record DeleteServiceTermCommand(Guid ServiceTermId) : ICommand;
}
