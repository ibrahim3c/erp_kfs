using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.GetJobGradeList
{
    public sealed class GetJobGradeListQueryHandler
        : IQueryHandler<GetJobGradeListQuery, List<GetJobGradeListResponse>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetJobGradeListQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<List<GetJobGradeListResponse>>> Handle(
            GetJobGradeListQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = """
                SELECT
                    "Id"          AS Id,
                    "Code"        AS Code,
                    "Name"        AS Name,
                    "GradeLevel"  AS GradeLevel,
                    "Description" AS Description,
                    "YearsNo"     AS YearsNo,
                    "IsActive"    AS IsActive
                FROM "HR"."JobGrades"
                ORDER BY "GradeLevel" ASC;
                """;

            var result = (await connection.QueryAsync<GetJobGradeListResponse>(sql)).ToList();
            return Result<List<GetJobGradeListResponse>>.Success(result);
        }
    }
}
