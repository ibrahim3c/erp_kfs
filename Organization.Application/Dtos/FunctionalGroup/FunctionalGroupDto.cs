namespace Organization.Application.Dtos.FunctionalGroup
{
    public record FunctionalGroupDto(
        Guid Id,
        Guid QualitativeGroupId,
        string QualitativeGroupName,
        string Code,
        string Name,
        string Description,
        bool IsActive);
}