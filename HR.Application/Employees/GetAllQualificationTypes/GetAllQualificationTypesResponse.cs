namespace HR.Application.Employees.GetAllQualificationTypes
{
    public sealed record GetAllQualificationTypesResponse(Guid id,string name,string description, bool isActive);
}
