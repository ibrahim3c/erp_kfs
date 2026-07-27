namespace HR.Domain.Leaves
{
    public interface ILeaveRepository
    {
        Task<LeaveRequest?> GetRequestByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<LeaveRequest>> GetRequestsByCategoryAsync(LeaveCategory category, CancellationToken ct = default);
        Task<IReadOnlyList<LeaveRequest>> GetRequestsByEmployeeAsync(Guid employeeId, int year, CancellationToken ct = default);
        void AddRequest(LeaveRequest request);
        void UpdateRequest(LeaveRequest request);

        Task<LeaveBalance?> GetBalanceAsync(Guid employeeId, int year, CancellationToken ct = default);
        void AddBalance(LeaveBalance balance);
        void UpdateBalance(LeaveBalance balance);
    }
}
