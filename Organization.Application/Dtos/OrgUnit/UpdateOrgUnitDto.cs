namespace Organization.Application.Dtos.OrgUnit
{
    public record UpdateOrgUnitDto(Guid Id, string Name, string Code, Guid OrgUnitTypeId, Guid? ParentId, Guid? GovernorateId);
}