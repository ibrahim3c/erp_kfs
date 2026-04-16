using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.GetJobTitleList
{
    public sealed class GetJobTitleListQueryHandler
        : IQueryHandler<GetJobTitleListQuery, List<GetJobTitleListResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetJobTitleListQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<GetJobTitleListResponse>>> Handle(
            GetJobTitleListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = """
                SELECT
                    jt."Id"                  AS Id,
                    jt."Code"                AS Code,
                    jt."Name"                AS Name,
                    jt."Description"         AS Description,
                    jt."FunctionalGroupId"   AS FunctionalGroupId,
                    fg."Name"                AS FunctionalGroupName,
                    qg."Name"                AS QualitativeGroupName,
                    jt."IsActive"            AS IsActive
                FROM "HR"."JobTitles"        jt
                INNER JOIN "HR"."FunctionalGroups"  fg ON jt."FunctionalGroupId" = fg."Id"
                INNER JOIN "HR"."QualitativeGroups" qg ON fg."QualitativeGroupId" = qg."Id"
                ORDER BY qg."Name", jt."Name";
                """;

            var result = (await connection.QueryAsync<GetJobTitleListResponse>(sql)).ToList();
            return Result<List<GetJobTitleListResponse>>.Success(result);
        }
    }
}
