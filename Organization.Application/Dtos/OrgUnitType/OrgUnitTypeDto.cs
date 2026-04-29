namespace Organization.Application.Dtos.OrgUnitType
{
    public record OrgUnitTypeDto(Guid Id, string Code, string Name, int LevelOrder, bool CanHaveChild);
}