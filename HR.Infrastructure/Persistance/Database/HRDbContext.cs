using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.JobStructures;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Infrastructure.Database;
using System.Reflection;
namespace HR.Infrastructure.Persistance.Database
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        // candidate
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<NominationFile> NominationFiles { get; set; }
        // employee
        public DbSet<Employee> Employees { get; set; }
        // jobStructure
        public DbSet<FunctionalGroup> FunctionalGroups { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<QualitativeGroup> QualitativeGroups { get; set; }
        // loans
        public DbSet<Loan> Loans { get; set; }
        public DbSet<InsurancePeriodPurchase> InsurancePeriodPurchases { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        //penalty
        public DbSet<PenaltyRecord> PenaltyRecords { get; set; }
        // payroll
        public DbSet<PayrollAdjustment> PayrollAdjustments  { get; set; }
        public DbSet<PayrollCycle> PayrollCycles { get; set; }
        public DbSet<PayrollEntry> PayrollEntries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تحديد الـ Schema الافتراضية
            modelBuilder.HasDefaultSchema(Schemas.HR);

            // هذا السطر يقوم بقراءة جميع كلاسات الـ Configuration اللي عملناها فوق وتطبيقها تلقائياً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
