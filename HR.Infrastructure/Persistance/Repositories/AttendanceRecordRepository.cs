using HR.Domain.Attendance;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        private readonly HRDbContext _dbContext;

        public AttendanceRecordRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(AttendanceRecord record)
        {
            _dbContext.AttendanceRecords.Add(record);
        }

        public void Update(AttendanceRecord record)
        {
            _dbContext.AttendanceRecords.Update(record);
        }

        public async Task<AttendanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceRecords.FindAsync(id, cancellationToken);
        }

        public async Task<AttendanceRecord?> GetByEmployeeAndDateAsync(
            Guid employeeId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceRecords
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date == date.Date, cancellationToken);
        }

        public async Task<List<AttendanceRecord>> GetByDateAsync(
            DateTime date, Guid? orgUnitId = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.AttendanceRecords
                .Include(x => x.Employee)
                .Where(x => x.Date == date.Date);

            if (orgUnitId.HasValue && orgUnitId.Value != Guid.Empty)
                query = query.Where(x => x.Employee.OrgUnitId == orgUnitId.Value);

            return await query
                .OrderBy(x => x.Employee.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AttendanceRecord>> GetByDateRangeAndEmployeeAsync(
            Guid employeeId, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceRecords
                .Where(x => x.EmployeeId == employeeId
                    && x.Date >= dateFrom.Date
                    && x.Date <= dateTo.Date)
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalWorkforceAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees.CountAsync(e => e.IsActive, cancellationToken);
        }

        public async Task<int> GetPresentCountAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceRecords
                .CountAsync(x => x.Date == date.Date && x.Status == AttendanceStatus.Present, cancellationToken);
        }

        public async Task<int> GetLateCountAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceRecords
                .CountAsync(x => x.Date == date.Date && x.Status == AttendanceStatus.Late, cancellationToken);
        }

        public async Task<int> GetAbsentCountAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            var total = await GetTotalWorkforceAsync(cancellationToken);
            var present = await GetPresentCountAsync(date, cancellationToken);
            var late = await GetLateCountAsync(date, cancellationToken);
            return total - present - late;
        }
    }
}
