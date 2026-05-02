namespace HR.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);

        void Add(Employee employee);
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        void Update(Employee employee);
        void Delete(Employee employee);

        Task<string> GetNextCodeAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct = default);
    }
}
