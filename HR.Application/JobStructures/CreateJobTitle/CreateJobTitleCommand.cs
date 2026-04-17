using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobTitle
{
    public record CreateJobTitleCommand(
       Guid FunctionalGroupId,
       string Code,
       string Name,
       string Description
   ) : ICommand<Guid>;
}
