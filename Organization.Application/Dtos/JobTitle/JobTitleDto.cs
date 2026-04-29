namespace Organization.Application.Dtos.JobTitle
{
    public record JobTitleDto(
        Guid Id,
        Guid FunctionalGroupId,
        string FunctionalGroupName,
        string Code,
        string Name,
        string Description,
        bool IsActive);
}