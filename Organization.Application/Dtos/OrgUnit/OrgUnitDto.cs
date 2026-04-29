namespace Organization.Application.Dtos.OrgUnit
{
    public record OrgUnitDto(
        Guid Id,
        string Name,
        string Code,
        Guid OrgUnitTypeId,
        string OrgUnitTypeName,
        Guid? ParentId,
        string? ParentName,
        Guid? GovernorateId,
        string? GovernorateName,
        bool IsActive);
}