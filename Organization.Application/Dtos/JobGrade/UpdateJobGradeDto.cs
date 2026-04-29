namespace Organization.Application.Dtos.JobGrade
{
    public record UpdateJobGradeDto(Guid Id, string Code, string Name, int GradeLevel, string Description, int YearsNo);
}