namespace HR.Application.Departments.GetDepartmentStats
{
    public class GetDepartmentStatsResponse
    {
        public int TotalUnits { get; init; }
        public int ActiveUnits { get; init; }
        public int DepartmentCount { get; init; }
        public int SectionCount { get; init; }
        public int TotalEmployees { get; init; }
    }
}
