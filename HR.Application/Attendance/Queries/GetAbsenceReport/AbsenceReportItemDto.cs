namespace HR.Application.Attendance.Queries.GetAbsenceReport
{
    public class AbsenceReportItemDto
    {
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string JobTitleName { get; init; } = string.Empty;
        public string DepartmentName { get; init; } = string.Empty;
        public int AbsenceDays { get; init; }
        public List<string> AbsentDates { get; set; } = new();
    }
}
