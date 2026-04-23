using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Permissions.GetAttendanceLog
{
    public class AttendanceLogItem
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty; // Permission / Late
        public string SubType { get; init; } = string.Empty; // Personal / Official / Medical
        public string TimeRange { get; init; } = string.Empty; // "10:00 : 12:00"
        public int DurationMinutes { get; init; }
        public string? Notes { get; init; }
        public string StatusLabel { get; init; } = string.Empty;
        public bool IsTransferred { get; init; }
    }
}
