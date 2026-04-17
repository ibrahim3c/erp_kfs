using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayrollCycle
{
    public record GetPayrollCycleQuery(int Month, int Year)
         : IQuery<GetPayrollCycleResponse?>;
}
