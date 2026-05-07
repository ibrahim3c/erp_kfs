namespace HR.Application.Attendance.Queries.GetAbsenceReport
{
    public class AbsenceReportResponse
    {
        public DateTime DateFrom { get; init; }
        public DateTime DateTo { get; init; }
        public int TotalAbsenceDays { get; init; }
        public int AffectedEmployeesCount { get; init; }
        public List<AbsenceReportItemDto> Items { get; init; } = new();
    }
}
