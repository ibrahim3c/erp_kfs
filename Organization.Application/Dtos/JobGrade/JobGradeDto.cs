namespace Organization.Application.Dtos.JobGrade
{
    public record JobGradeDto(Guid Id, string Code, string Name, int GradeLevel, string Description, int YearsNo, bool IsActive);
}