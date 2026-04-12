using Modules.Shared.Application.Messaging;

namespace HR.Application.Employees.CreateEmployee
{
    public sealed record CreateEmployeeCommand(
            string Name,
            string NationalId,
            DateTime HireDate,
            string? Phone,
            DateTime? DateOfBirth,
            string? Gender,
            string? Email,
            string? Address,
            string? MaritalStatus,
            Guid? CityCenterId,
            Guid? VillageId,
            Guid? QualificationTypeId,
            string? Specialization,
            Guid? EmploymentTypeId,
            Guid? JobTitleId,
            Guid? JobGradeId,
            Guid? FunctionalGroupId,
            Guid? OrgUnitId
        ) : ICommand<Guid>;
}
