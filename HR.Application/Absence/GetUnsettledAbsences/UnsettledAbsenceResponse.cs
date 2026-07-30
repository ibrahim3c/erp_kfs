namespace HR.Application.Absence.GetUnsettledAbsences
{
    public class UnsettledAbsenceResponse
    {
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public string AbsentDates { get; init; } = string.Empty;
        public int AbsenceDays { get; init; }
        public string AbsenceType { get; init; } = string.Empty;
        public int RegularBalance { get; init; }
        public string CurrentAction { get; init; } = string.Empty;
        public string ActionType { get; init; } = string.Empty;
        public bool IsOverLegalLimit { get; init; }
    }
}
