using MediatR;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Command.UpdateFinancialData
{
    public record UpdateFinancialDataCommand(Guid RetirementFileId, Dictionary<int, decimal> YearAmounts) : ICommand;
}
 