using HR.Domain.Attendance;
using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Application.Exceptions;
using Modules.Shared.Domain;
using Modules.Shared.Infrastructure.Database;
using System.Linq.Expressions;
using System.Reflection;

namespace HR.Infrastructure.Persistance.Database;

public class HRDbContext : DbContext
{
    private readonly IMediator _mediator;

    public HRDbContext(DbContextOptions<HRDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<NominationFile> NominationFiles { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeFile> EmployeeFiles { get; set; }
    public DbSet<EmployeeFinancial> EmployeeFinancials { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<InsurancePeriodPurchase> InsurancePeriodPurchases { get; set; }
    public DbSet<LoanInstallment> LoanInstallments { get; set; }
    public DbSet<PenaltyRecord> PenaltyRecords { get; set; }
    public DbSet<PayrollAdjustment> PayrollAdjustments { get; set; }
    public DbSet<PayrollCycle> PayrollCycles { get; set; }
    public DbSet<PayrollEntry> PayrollEntries { get; set; }
    public DbSet<PermissionRequest> PermissionRequests { get; set; }
    public DbSet<LateEntry> LateEntries { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.HR);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    //  Override SaveChangesAsync لإطلاق الـ Domain Events
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<PayrollAdjustment>())
            {
                Console.WriteLine($"=== ENTRY STATE: {entry.State} ===");
                Console.WriteLine($"    ID: {entry.Entity.Id}");
                Console.WriteLine($"    EntryId: {entry.Entity.EntryId}");

                // الأهم — شوف الـ original values
                if (entry.State == EntityState.Modified)
                {
                    Console.WriteLine("    --- ORIGINAL VALUES ---");
                    foreach (var prop in entry.OriginalValues.Properties)
                        Console.WriteLine($"    {prop.Name}: {entry.OriginalValues[prop]}");

                    Console.WriteLine("    --- CURRENT VALUES ---");
                    foreach (var prop in entry.CurrentValues.Properties)
                        Console.WriteLine($"    {prop.Name}: {entry.CurrentValues[prop]}");
                }
            }
            // 1. اجمع الـ Events قبل الحفظ
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e =>
                {
                    var events = e.GetDomainEvents().ToList();
                    e.ClearDomainEvents();
                    return events;
                })
                .ToList();

            // 2. احفظ الداتا
            var result = await base.SaveChangesAsync(cancellationToken);

            // 3. أطلق الـ Events بعد الحفظ
            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // هنجيب أول كيان حصلت فيه المشكلة
            var entry = ex.Entries.FirstOrDefault();

            // هنجيب اسم الكلاس (الجدول) اللي ضرب
            var entityName = entry?.Metadata.ClrType.Name ?? "Unknown Entity";

            // هنجيب حالته (هل كان بيعمل Update ولا Insert ولا Delete)
            var state = entry?.State.ToString() ?? "Unknown State";

            // هنرمي الخطأ تاني بس هنحط فيه التفاصيل دي عشان تظهرلك في الشاشة
            throw new ConcurrencyException($"Concurrency error on Entity: '{entityName}' | State: '{state}'.", ex);
        }
    }
}