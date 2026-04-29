namespace Organization.Application.Dtos.OrgUnit
{
    public record CreateOrgUnitDto(string Name, string Code, Guid OrgUnitTypeId, Guid? ParentId, Guid? GovernorateId);
}