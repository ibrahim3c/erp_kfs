using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Absence.GetUnsettledAbsences
{
    public sealed class GetUnsettledAbsencesQueryHandler
        : IQueryHandler<GetUnsettledAbsencesQuery, List<UnsettledAbsenceResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        private const int AnnualRegularLeave = 21;
        private const int AnnualCasualLeave = 7;

        public GetUnsettledAbsencesQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<UnsettledAbsenceResponse>>> Handle(
            GetUnsettledAbsencesQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var dateFrom = new DateTime(request.Year, request.Month, 1);
            var dateTo = dateFrom.AddMonths(1).AddDays(-1);
            var yearStart = new DateTime(request.Year, 1, 1);

            const string sql = """
                SELECT
                    e.Id AS EmployeeId,
                    e.Name AS EmployeeName,
                    COUNT(ar.Id) AS AbsenceDays,
                    STRING_AGG(CONVERT(NVARCHAR(10), ar.Date, 103), N', ') AS AbsentDates
                FROM HR.AttendanceRecords ar
                INNER JOIN HR.Employees e ON e.Id = ar.EmployeeId
                WHERE ar.Date BETWEEN @DateFrom AND @DateTo
                  AND ar.Status = 3
                  AND e.IsActive = 1
                GROUP BY e.Id, e.Name
                ORDER BY COUNT(ar.Id) DESC
                """;

            var parameters = new DynamicParameters();
            parameters.Add("DateFrom", dateFrom);
            parameters.Add("DateTo", dateTo);

            var items = (await connection.QueryAsync(sql, parameters)).ToList();

            var result = new List<UnsettledAbsenceResponse>();

            foreach (var row in items)
            {
                var employeeId = (Guid)row.EmployeeId;
                var absenceDays = (int)row.AbsenceDays;
                var employeeName = (string)row.EmployeeName;
                var absentDates = (string?)row.AbsentDates ?? "";

                var usedRegular = await connection.ExecuteScalarAsync<int>(
                    """
                    SELECT COUNT(1) FROM HR.AttendanceRecords
                    WHERE EmployeeId = @EmpId AND Date BETWEEN @YearStart AND @YearEnd AND Status = 5
                    """,
                    new { EmpId = employeeId, YearStart = yearStart, YearEnd = DateTime.UtcNow.Date });

                var remainingBalance = AnnualRegularLeave - usedRegular;
                if (remainingBalance < 0) remainingBalance = 0;

                var totalYearAbsence = await connection.ExecuteScalarAsync<int>(
                    """
                    SELECT COUNT(1) FROM HR.AttendanceRecords
                    WHERE EmployeeId = @EmpId AND Date BETWEEN @YearStart AND @YearEnd AND Status = 3
                    """,
                    new { EmpId = employeeId, YearStart = yearStart, YearEnd = DateTime.UtcNow.Date });

                var isOverLegalLimit = totalYearAbsence >= 10;

                var actionType = isOverLegalLimit ? "LegalWarning" : "Pending";
                var currentAction = isOverLegalLimit ? "يجب الإنذار" : "معلق";

                result.Add(new UnsettledAbsenceResponse
                {
                    EmployeeId = employeeId,
                    EmployeeName = employeeName,
                    AbsentDates = absentDates,
                    AbsenceDays = absenceDays,
                    AbsenceType = "منقطع (متصل)",
                    RegularBalance = remainingBalance,
                    CurrentAction = currentAction,
                    ActionType = actionType,
                    IsOverLegalLimit = isOverLegalLimit
                });
            }

            return Result<List<UnsettledAbsenceResponse>>.Success(result);
        }
    }
}
