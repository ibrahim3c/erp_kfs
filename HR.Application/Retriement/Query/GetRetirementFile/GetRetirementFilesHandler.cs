using Dapper;
using HR.Domain.Retirement.Entities;
using HR.Domain.Retirement.Enums;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFile
{
    public class GetRetirementFilesHandler : IQueryHandler<GetRetirementFilesQuery, RetirementFilesResult>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetRetirementFilesHandler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<RetirementFilesResult>> Handle(GetRetirementFilesQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string listSql = """
            SELECT rf.Id, rf.EmployeeId, e.Name AS EmployeeName, rf.ReferralDate,
                   rf.Reason, rf.Stage, rf.UpdatedOn, resp.Name AS ResponsibleName
            FROM HR.RetirementFiles rf

            JOIN HR.Employees e ON e.Id = rf.EmployeeId
            LEFT JOIN HR.Employees resp ON resp.Id = rf.ResponsibleEmployeeId

            ORDER BY rf.UpdatedOn DESC
            """;

            const string countsSql =
               """
                SELECT Stage, COUNT(*) AS Total
                FROM HR.RetirementFiles
                GROUP BY Stage            
                """;

            var files = (await connection.QueryAsync<RetirementFileListItemDto>(listSql)).ToList();
            var counts = (await connection.QueryAsync<(string Stage, int Total)>(countsSql))
                .ToDictionary(x => x.Stage, x => x.Total);

            var result = new RetirementFilesResult(
                files,
                counts.GetValueOrDefault(ToArabicText(RetirementStage.UnderFinancialReview)),
                counts.GetValueOrDefault(ToArabicText(RetirementStage.AwaitingSignatures)),
                counts.GetValueOrDefault(ToArabicText(RetirementStage.DeliveredToAuthority)),
                counts.GetValueOrDefault(ToArabicText(RetirementStage.Rejected)));

            return Result<RetirementFilesResult>.Success(result);
        }
        private static string ToArabicText(RetirementStage stage) => stage switch
        {
            RetirementStage.PendingReview => "مراجعة التدرج الوظيفي",
            RetirementStage.UnderFinancialReview => "تحت المراجعة المالية",
            RetirementStage.AwaitingSignatures => "في انتظار التوقيعات",
            RetirementStage.DeliveredToAuthority => "تم تسليم الملف (أرشيف)",
            RetirementStage.Rejected => "مرتد",
            _ => stage.ToString()
        };

    }

}
