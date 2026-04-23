using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.GetAttendanceLog
{
    public class GetAttendanceLogResponse
    {
        // Summary Cards
        public int TotalPermissionMinutes { get; init; }
        public int TotalLateMinutes { get; init; }
        public int EmployeesExceededLimit { get; init; }

        // جدول الحركات
        public List<AttendanceLogItem> Items { get; init; } = new();
    }
}
