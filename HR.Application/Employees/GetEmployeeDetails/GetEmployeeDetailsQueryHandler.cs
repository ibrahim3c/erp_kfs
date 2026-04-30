using Dapper;
using HR.Domain.Employees;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Employees.GetEmployeeDetails
{
    public sealed class GetEmployeeDetailsQueryHandler : IQueryHandler<GetEmployeeDetailsQuery, GetEmployeeDetailsResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetEmployeeDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetEmployeeDetailsResponse>> Handle(GetEmployeeDetailsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            // Note: Adjust the table name ("Employees") and column names if they differ in your actual database schema
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

                        jt.Name  AS JobTitleName,
                        jg.Name  AS JobGradeName,
                        et.Name  AS EmploymentTypeName,
                        ou.Name  AS OrgUnitName,
                        fg.Name  AS FunctionalGroupName,

                        ef.GrossSalary,
                        ef.BasicSalary2019,
                        ef.InsuranceNumber,
                        ef.BankName,
                        ef.BankAccount,
                        ef.HasFellowshipFund,
                        ef.HasMedicalFund,

                        efl.PersonalPhoto,
                        efl.NationalIdCardFront,
                        efl.QualificationFile,
                        efl.BirthCertificateFile,
                        efl.MilitaryFile,
                        efl.ContractFile,
                        efl.PoliceClearanceCertificate

                    FROM HR.Employees e
                    LEFT JOIN Organization.JobTitles       jt  ON jt.Id  = e.JobTitleId
                    LEFT JOIN Organization.JobGrades       jg  ON jg.Id  = e.JobGradeId
                    LEFT JOIN HR.EmploymentTypes           et  ON et.Id  = e.EmploymentTypeId
                    LEFT JOIN Organization.OrgUnits        ou  ON ou.Id  = e.OrgUnitId
                    LEFT JOIN Organization.FunctionalGroups fg ON fg.Id  = e.FunctionalGroupId
                    LEFT JOIN HR.EmployeeFinancials        ef  ON ef.EmployeeId = e.Id
                    LEFT JOIN HR.EmployeeFiles             efl ON efl.EmployeeId = e.Id
                    WHERE e.Id = @EmployeeId
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeDetailsResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeDetailsResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeDetailsResponse>.Success(response);
        }
    }
}
