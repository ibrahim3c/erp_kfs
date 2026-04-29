namespace Organization.Application.Dtos.JobGrade
{
    public record CreateJobGradeDto(string Code, string Name, int GradeLevel, string Description, int YearsNo);
}