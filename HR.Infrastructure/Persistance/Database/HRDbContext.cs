using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Infrastructure.Database;
using System.Reflection;

namespace HR.Infrastructure.Persistance.Database;

public class HRDbContext : DbContext
{
    public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<NominationFile> NominationFiles { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<InsurancePeriodPurchase> InsurancePeriodPurchases { get; set; }
    public DbSet<LoanInstallment> LoanInstallments { get; set; }
    public DbSet<PenaltyRecord> PenaltyRecords { get; set; }
    public DbSet<PayrollAdjustment> PayrollAdjustments { get; set; }
    public DbSet<PayrollCycle> PayrollCycles { get; set; }
    public DbSet<PayrollEntry> PayrollEntries { get; set; }
    public DbSet<PermissionRequest> PermissionRequests { get; set; }
    public DbSet<LateEntry> LateEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.HR);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}