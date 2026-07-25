using MediatR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFileDetails
{
    public record GetRetirementFileDetailsQuery(Guid RetirementFileId) : IQuery<RetirementFileDetailsDto>;
}
