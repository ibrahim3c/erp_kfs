namespace Organization.Application.Dtos.JobTitle
{
    public record UpdateJobTitleDto(Guid Id, Guid FunctionalGroupId, string Code, string Name, string Description);
}