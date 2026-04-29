namespace Organization.Application.Dtos.JobTitle
{
    public record CreateJobTitleDto(Guid FunctionalGroupId, string Code, string Name, string Description);
}