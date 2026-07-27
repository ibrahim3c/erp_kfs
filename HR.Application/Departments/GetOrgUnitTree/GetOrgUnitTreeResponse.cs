namespace HR.Application.Departments.GetOrgUnitTree
{
    public class GetOrgUnitTreeResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public Guid? ParentId { get; init; }
        public string? ParentName { get; init; }
        public Guid OrgUnitTypeId { get; init; }
        public string OrgUnitTypeName { get; init; } = string.Empty;
        public int LevelOrder { get; init; }
        public bool IsActive { get; init; }
        public string? CurrentManagerName { get; init; }
        public int EmployeeCount { get; init; }
    }
}
