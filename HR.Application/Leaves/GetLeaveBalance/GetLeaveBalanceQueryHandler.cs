using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.GetLeaveBalance
{
    public sealed class GetLeaveBalanceQueryHandler
        : IQueryHandler<GetLeaveBalanceQuery, GetLeaveBalanceResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetLeaveBalanceQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetLeaveBalanceResponse>> Handle(
            GetLeaveBalanceQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var year = DateTime.Now.Year;

            const string sql = """
                SELECT
                    lb.RegularLeaveEntitled,
                    lb.RegularLeaveUsed,
                    (lb.RegularLeaveEntitled + lb.CarryOverRegularDays - lb.RegularLeaveUsed) AS RegularRemaining,
                    lb.CasualLeaveEntitled,
                    lb.CasualLeaveUsed,
                    (lb.CasualLeaveEntitled - lb.CasualLeaveUsed) AS CasualRemaining,
                    lb.CarryOverRegularDays
                FROM HR.LeaveBalances lb
                WHERE lb.EmployeeId = @EmployeeId AND lb.Year = @Year
                """;

            var response = await connection.QueryFirstOrDefaultAsync<GetLeaveBalanceResponse>(
                sql, new { request.EmployeeId, Year = year });

            response ??= new GetLeaveBalanceResponse
            {
                RegularLeaveEntitled = 21,
                RegularLeaveUsed = 0,
                RegularRemaining = 21,
                CasualLeaveEntitled = 7,
                CasualLeaveUsed = 0,
                CasualRemaining = 7,
                CarryOverRegularDays = 0
            };

            return Result<GetLeaveBalanceResponse>.Success(response);
        }
    }
}
