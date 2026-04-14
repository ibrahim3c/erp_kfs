namespace HR.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);

        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);

        Task<string> GetNextCodeAsync(CancellationToken cancellationToken = default);
    }
}
