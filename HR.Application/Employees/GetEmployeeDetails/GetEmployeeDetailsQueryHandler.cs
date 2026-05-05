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
            e.IsActive,
            e.IsDisabled,
            e.HireDate,
            e.DateOfBirth,

            jt.Name AS JobTitleName,
            jg.Name AS JobGradeName,
            et.Name AS EmploymentTypeName,
            ou.Name AS OrgUnitName,
            fg.Name AS FunctionalGroupName,

            qt.Id AS QualificationTypeId,
            qt.Name AS QualificationTypeName,

            eq.QualificationFullName,
            eq.Specialization AS QualificationSpecialization,
            eq.University AS QualificationUniversity,
            eq.GraduationYear AS QualificationGraduationYear,
            eq.Grade AS QualificationGrade,
            eq.IsVerified AS QualificationIsVerified,
            eq.ValidFrom AS QualificationValidFrom,
            eq.ValidTo AS QualificationValidTo,
            eq.Notes AS QualificationNotes,

            ef.GrossSalary,
            ef.BasicSalary2019,
            ef.InsuranceNumber,
            ef.BankName,
            ef.BankAccount,
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
                        efl.MarriageDocument,

                        eq.QualificationFullName,
                        qt.Name  AS QualificationTypeName,
                        eq.Specialization,
                        eq.University,
                        eq.GraduationYear,
                        eq.Grade,
                        eq.ValidFrom AS QualificationValidFrom,
                        eq.ValidTo   AS QualificationValidTo,
                        eq.Notes     AS QualificationNotes

                    FROM HR.Employees e
                    LEFT JOIN Organization.JobTitles        jt  ON jt.Id  = e.JobTitleId
                    LEFT JOIN Organization.JobGrades        jg  ON jg.Id  = e.JobGradeId
                    LEFT JOIN HR.EmploymentTypes            et  ON et.Id  = e.EmploymentTypeId
                    LEFT JOIN Organization.OrgUnits         ou  ON ou.Id  = e.OrgUnitId
                    LEFT JOIN Organization.FunctionalGroups fg  ON fg.Id  = e.FunctionalGroupId
                    LEFT JOIN HR.EmployeeFinancials         ef  ON ef.EmployeeId  = e.Id
                    LEFT JOIN HR.EmployeeFiles              efl ON efl.EmployeeId = e.Id
                    LEFT JOIN HR.EmployeeQualifications     eq  ON eq.EmployeeId  = e.Id
                    LEFT JOIN HR.QualificationTypes         qt  ON qt.Id = eq.QualificationTypeId
                    WHERE e.Id = @EmployeeId
                    """;
            var response = await connection.QuerySingleOrDefaultAsync<GetEmployeeDetailsResponse>(sql, new { request.EmployeeId });

            if (response is null)
                return Result<GetEmployeeDetailsResponse>.Failure(EmployeeErrors.NotFound);

            return Result<GetEmployeeDetailsResponse>.Success(response);
        }
    }
}
