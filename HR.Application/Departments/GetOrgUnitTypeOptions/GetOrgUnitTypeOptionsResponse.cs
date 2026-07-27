namespace HR.Application.Departments.GetOrgUnitTypeOptions
{
    public class GetOrgUnitTypeOptionsResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int LevelOrder { get; init; }
        public bool CanHaveChild { get; init; }
    }
}
