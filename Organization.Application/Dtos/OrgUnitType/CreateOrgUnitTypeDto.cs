namespace Organization.Application.Dtos.OrgUnitType
{
    public record CreateOrgUnitTypeDto(string Code, string Name, int LevelOrder, bool CanHaveChild);
}