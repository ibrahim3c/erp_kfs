namespace Organization.Application.Dtos.QualitativeGroup
{
    public record QualitativeGroupDto(Guid Id, string Code, string Name, string Description, bool IsActive);
}