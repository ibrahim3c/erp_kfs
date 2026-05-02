using Dapper;
using HR.Domain.Employees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.GetEmployeeForEdit
{
    public sealed class GetEmployeeForEditQueryHandler : IQueryHandler<GetEmployeeForEditQuery, GetEmployeeForEditResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetEmployeeForEditQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetEmployeeForEditResponse>> Handle(GetEmployeeForEditQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                    SELECT
                        e.Id,
                        e.Code,
                        e.Name,
                        e.Phone,
                        e.Email,
                        e.NationalId,
                        e.Gender,
                        e.Address,
                        e.MaritalStatus,
                        e.IsActive,
                        e.IsDisabled,
                        e.HireDate,
                        e.DateOfBirth,
                        e.JobGradeDate,
                        e.OrgUnitId,
                        e.JobGradeId,
                        e.EmploymentTypeId,
                        e.FunctionalGroupId,
                        e.JobTitleName,
                        e.QualificationName,

                        ef.GrossSalary,
                        ef.BasicSalary2019,
                        ef.InsuranceNumber,
                        ef.BankName,
                        ef.BankAccount      AS BankAccountNumber,
                        ef.HasFellowshipFund,
                        ef.HasMedicalFund

                    FROM HR.Employees e
                    LEFT JOIN HR.EmployeeFinancials ef ON ef.EmployeeId = e.Id
                    WHERE e.Id = @EmployeeId
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeForEditResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeForEditResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeForEditResponse>.Success(response);
        }
    }
}