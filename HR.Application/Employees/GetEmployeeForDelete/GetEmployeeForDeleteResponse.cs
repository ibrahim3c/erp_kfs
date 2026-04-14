namespace HR.Application.Employees.GetEmployeeForDelete
{
    public sealed class GetEmployeeForDeleteResponse
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Email { get; private set; }
    }
}
