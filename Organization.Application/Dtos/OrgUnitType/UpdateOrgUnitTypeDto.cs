namespace Organization.Application.Dtos.OrgUnitType
{
    public record UpdateOrgUnitTypeDto(Guid Id, string Code, string Name, int LevelOrder, bool CanHaveChild);
}