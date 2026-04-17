using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.UpdateJobTitle
{
    public record UpdateJobTitleCommand(
       Guid Id,
       string Code,
       string Name,
       string Description
   ) : ICommand;
}
