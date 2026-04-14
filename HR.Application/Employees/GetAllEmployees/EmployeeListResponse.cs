namespace HR.Application.Employees.GetAllEmployees
{
    public sealed class EmployeeListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime HireDate { get; private set; }
        public bool IsActive { get; private set; }
    }
}
