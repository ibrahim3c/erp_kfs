using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System.Text;

namespace HR.Application.Evaluations.GetKpiReportList
{
    public sealed class GetKpiReportListQueryHandler
        : IQueryHandler<GetKpiReportListQuery, List<GetKpiReportListResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetKpiReportListQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetKpiReportListResponse>>> Handle(
            GetKpiReportListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    k.Id,
                    k.EmployeeId,
                    e.Name AS EmployeeName,
                    jg.Name AS JobGradeName,
                    k.Year,
                    k.Score,
                    k.EfficiencyScore,
                    k.DisciplineScore,
                    k.AchievementScore,
                    k.Grade,
                    ev.Name AS EvaluatorName,
                    k.Status
                FROM HR.KpiReports k
                INNER JOIN HR.Employees e ON e.Id = k.EmployeeId
                LEFT JOIN HR.JobGrades jg ON jg.Id = e.JobGradeId
                LEFT JOIN HR.Employees ev ON ev.Id = k.EvaluatorId");

            if (request.Year.HasValue)
            {
                sql.Append(" WHERE k.Year = @Year");
            }

            sql.Append(" ORDER BY k.Year DESC, e.Name ASC");

            var parameters = new { Year = request.Year };

            var response = (await connection.QueryAsync<GetKpiReportListResponse>(sql.ToString(), parameters)).ToList();

            return Result<List<GetKpiReportListResponse>>.Success(response);
        }
    }
}
