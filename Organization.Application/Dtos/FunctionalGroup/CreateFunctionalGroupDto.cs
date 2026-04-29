namespace Organization.Application.Dtos.FunctionalGroup
{
    public record CreateFunctionalGroupDto(Guid QualitativeGroupId, string Code, string Name, string Description);
}