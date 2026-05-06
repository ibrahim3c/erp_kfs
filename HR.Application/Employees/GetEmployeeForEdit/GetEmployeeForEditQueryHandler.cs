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
                    e.OrgUnitId,
                    e.JobTitleId,
                    e.JobGradeId,
                    e.EmploymentTypeId,
                    e.FunctionalGroupId,

                    jt.Name  AS JobTitleName,
                    qt.Name  AS QualificationTypeName,

                    ef.GrossSalary,
                    ef.BasicSalary2019,
                    ef.Incentives,
                    ef.InsuranceNumber,
                    ef.BankName,
                    ef.BankAccount      AS BankAccountNumber,
                    ef.HasFellowshipFund,
                    ef.HasMedicalFund,

                efl.PersonalPhoto,
                efl.NationalIdCardFront,
                efl.NationalIdCardBack,
                efl.QualificationFile,
                efl.BirthCertificateFile,
                efl.MilitaryFile,
                efl.ContractFile,
                efl.PoliceClearanceCertificate,
                efl.MarriageDocument

                FROM HR.Employees e
                LEFT JOIN Organization.JobTitles        jt  ON jt.Id = e.JobTitleId
                LEFT JOIN HR.EmployeeFinancials         ef  ON ef.EmployeeId = e.Id
                LEFT JOIN HR.EmployeeQualifications     eq  ON eq.EmployeeId = e.Id
                LEFT JOIN HR.QualificationTypes         qt  ON qt.Id = eq.QualificationTypeId
                LEFT JOIN HR.EmployeeFiles              efl ON efl.EmployeeId = e.Id
                WHERE e.Id = @EmployeeId
                """;

            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeForEditResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeForEditResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeForEditResponse>.Success(response);
        }
    }
}