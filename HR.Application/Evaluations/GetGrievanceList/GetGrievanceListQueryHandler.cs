using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Evaluations.GetGrievanceList
{
    public sealed class GetGrievanceListQueryHandler
        : IQueryHandler<GetGrievanceListQuery, List<GetGrievanceListResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetGrievanceListQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<List<GetGrievanceListResponse>>> Handle(
            GetGrievanceListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    g.Id,
                    g.EmployeeId,
                    e.Name AS EmployeeName,
                    g.GrievanceType AS GrievanceTypeName,
                    g.ComplainedDecisionNumber,
                    g.ComplainedDecisionDate,
                    g.SubmissionDate,
                    g.Reasons,
                    CASE WHEN g.AttachmentPath IS NOT NULL THEN 1 ELSE 0 END AS HasAttachment,
                    g.Status AS StatusName,
                    g.CommitteeNotes,
                    g.ResolutionDate
                FROM HR.Grievances g
                INNER JOIN HR.Employees e ON e.Id = g.EmployeeId
                ORDER BY g.SubmissionDate DESC
                """;

            var response = (await connection.QueryAsync<GetGrievanceListResponse>(sql)).ToList();

            return Result<List<GetGrievanceListResponse>>.Success(response);
        }
    }
}
