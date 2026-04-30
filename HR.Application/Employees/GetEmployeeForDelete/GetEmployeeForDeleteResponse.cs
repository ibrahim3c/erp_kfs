namespace HR.Application.Employees.GetEmployeeForDelete
{

    public sealed class GetEmployeeForDeleteResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public string? Email { get; init; }
    }
}
