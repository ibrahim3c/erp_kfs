namespace HR.Domain.Attendance
{
    public interface IAttendanceRecordRepository
    {
        Task<AttendanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AttendanceRecord?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date, CancellationToken cancellationToken = default);
        Task<List<AttendanceRecord>> GetByDateAsync(DateTime date, Guid? orgUnitId = null, CancellationToken cancellationToken = default);
        Task<List<AttendanceRecord>> GetByDateRangeAndEmployeeAsync(Guid employeeId, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default);
        Task<int> GetTotalWorkforceAsync(CancellationToken cancellationToken = default);
        Task<int> GetPresentCountAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<int> GetLateCountAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<int> GetAbsentCountAsync(DateTime date, CancellationToken cancellationToken = default);
        void Add(AttendanceRecord record);
        void Update(AttendanceRecord record);
    }
}
