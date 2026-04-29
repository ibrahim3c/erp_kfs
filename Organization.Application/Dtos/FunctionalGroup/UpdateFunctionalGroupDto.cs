namespace Organization.Application.Dtos.FunctionalGroup
{
    public record UpdateFunctionalGroupDto(Guid Id, Guid QualitativeGroupId, string Code, string Name, string Description);
}