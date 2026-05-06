using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.GetPayslip
{
    public record GetPayslipQuery(Guid EntryId) : IQuery<PayslipResponse?>;
}
