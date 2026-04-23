using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.GetAttendanceLog
{
    public record GetAttendanceLogQuery(int Month, int Year)
       : IQuery<GetAttendanceLogResponse>;
}
