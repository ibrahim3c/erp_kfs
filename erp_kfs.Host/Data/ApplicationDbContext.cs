using erp_kfs.Host.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Areas.Admin.Models;
using MyERP.Web.Areas.HR.Models;
using MyERP.Web.Areas.HR.Models.Hierarechy;
using MyERP.Web.Models;
using MyERP.Web.Models.Common;
using MyERP.Web.Models.SeedDataModels;

namespace MyERP.Web.Data
{
    // بنورث من IdentityDbContext عشان ينزل جداول المستخدمين (Users, Roles, Logins)

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // هنا بنعرف الجداول الخاصة بينا مستقبلاً
        // public DbSet<Employee> Employees { get; set; }
        // common in main project
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<CityCenter> CityCenters { get; set; }
        public DbSet<LocalUnit> LocalUnits { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Decision> Decisions { get; set; }

        //hierarechy
        public DbSet<OrgUnitType> OrgUnitTypes { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }

        
        // Admin DbSets
        public DbSet<QualitativeGroup> QualitativeGroups { get; set; }
        public DbSet<FunctionalGroup> FunctionalGroups { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<QualificationType> QualificationTypes { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<DecisionType> DecisionTypes { get; set; }
        public DbSet<DecisionAuthority> DecisionAuthorities { get; set; }

        public DbSet<LeadershipPosition> LeadershipPositions { get; set; }
        public DbSet<ServiceTerminationType> ServiceTerminationTypes { get; set; }

        // HR Dbsets
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeFamily> EmployeeFamilyMembers { get; set; }
        public DbSet<EmployeeQualification> EmployeeQualifications { get; set; }
        public DbSet<EmployeeFile> EmployeeFiles { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<NominationFile> NominationFiles { get; set; }
        public DbSet<EmployeeDecision> EmployeeDecisions { get; set; }
        public DbSet<AcademicIncentiveType> AcademicIncentiveTypes { get; set; }
        public DbSet<AcademicIncentiveRequest> AcademicIncentiveRequests { get; set; }
        public DbSet<LeadershipPositionHistory> LeadershipPositionHistories { get; set; }
        public DbSet<ServiceTerminationRequest> ServiceTerminationRequests { get; set; }

        // identity and Admin DbSets
        public DbSet<EmployeeAdmin> EmployeeAdmins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<GlobalLeadershipPosition> GlobalLeadershipPositions { get; set; }
        public DbSet<LeadershipAssignment> LeadershipAssignments { get; set; }
        public DbSet<ServicePeriodAddition> ServicePeriodAdditions { get; set; }
        public DbSet<TerminationRequest> TerminationRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seeeeds
            GovernorateSeed.Seed(builder);
            builder.Entity<Notification>()
                    .HasOne(n => n.SentByEmployee)
                    .WithMany()
                    .HasForeignKey(n => n.SentBy)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(n => n.SentToEmployee)
                .WithMany()
                .HasForeignKey(n => n.SentTo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AcademicIncentiveRequest>()
                     .HasOne(a => a.Employee)
                     .WithMany()
                     .HasForeignKey(a => a.EmployeeId)
                     .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AcademicIncentiveRequest>()
                .HasOne(a => a.Qualification)
                .WithMany()
                .HasForeignKey(a => a.QualificationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AcademicIncentiveRequest>()
                .HasOne(a => a.AcademicIncentiveType)
                .WithMany()
                .HasForeignKey(a => a.AcademicIncentiveTypeId)
                .OnDelete(DeleteBehavior.NoAction);


            // Employee → Department
            builder.Entity<EmployeeAdmin>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.SelectedDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Employee → LeaveRequests (تم إصلاح العلاقة)
            builder.Entity<EmployeeAdmin>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(lr => lr.Employee)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Department → Manager
            builder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // LeaveRequest → LeaveType
            builder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveType)
                .WithMany()
                .HasForeignKey(lr => lr.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // LeadershipAssignment → Position
            builder.Entity<LeadershipAssignment>()
                .HasOne(la => la.Position)
                .WithMany()
                .HasForeignKey(la => la.PositionId)
                .OnDelete(DeleteBehavior.Cascade);

            // LeadershipAssignment → Employee
            builder.Entity<LeadershipAssignment>()
                .HasOne(la => la.Employee)
                .WithMany()
                .HasForeignKey(la => la.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // TerminationRequest → Employee
            builder.Entity<TerminationRequest>()
                .HasOne(t => t.Employee)
                .WithMany()
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TerminationRequest → ServicePeriodAddition
            builder.Entity<TerminationRequest>()
                .HasOne(t => t.ServicePeriodAddition)
                .WithMany()
                .HasForeignKey(t => t.ServicePeriodAdditionId)
                .OnDelete(DeleteBehavior.NoAction);

            // ServicePeriodAddition → Employee
            builder.Entity<ServicePeriodAddition>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════
            // مفاتيح مركبة
            // ══════════════════════════════════════════════════════

            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            builder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany()
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════
            // بيانات تمهيدية (Seed Data)
            // ══════════════════════════════════════════════════════

            builder.Entity<Department>().HasData(
                new Department { Id = "governorate", Name = "ديوان عام المحافظة", Type = "General" },
                new Department { Id = "hr", Name = "الإدارة العامة للشئون الوظيفية", Type = "General" },
                new Department { Id = "finance", Name = "الإدارة العامة للشئون المالية", Type = "General" },
                new Department { Id = "engineering", Name = "الشئون الهندسية", Type = "General" },
                new Department { Id = "it", Name = "الإدارة العامة لنظم المعلومات والتحول الرقمي", Type = "General" }
            );

            builder.Entity<GlobalLeadershipPosition>().HasData(
                new GlobalLeadershipPosition
                {
                    Id = "pos-governor",
                    Title = "Governor",
                    DisplayName = "المحافظ",
                    Level = 1,
                    DepartmentId = "governorate"
                },
                new GlobalLeadershipPosition
                {
                    Id = "pos-deputy-governor",
                    Title = "DeputyGovernor",
                    DisplayName = "نائب المحافظ",
                    Level = 2,
                    DepartmentId = "governorate"
                },
                new GlobalLeadershipPosition
                {
                    Id = "pos-chief-secretary",
                    Title = "ChiefSecretary",
                    DisplayName = "السكرتير العام",
                    Level = 3,
                    DepartmentId = "governorate"
                },
                new GlobalLeadershipPosition
                {
                    Id = "pos-deputy-chief",
                    Title = "DeputyChiefSecretary",
                    DisplayName = "السكرتير العام المساعد",
                    Level = 4,
                    DepartmentId = "governorate"
                }
            );

            builder.Entity<LeaveType>().HasData(
                new LeaveType
                {
                    Id = "leave-annual",
                    Name = "Annual",
                    DisplayName = "الإجازة الاعتيادية",
                    MaxDays = 50,
                    RequiresApproval = true,
                    AutoRenewDate = "07-01",
                    IsAnnualBasedOnService = true,
                    SalaryPercentage = 100,
                    IsCasual = false
                },
                new LeaveType
                {
                    Id = "leave-casual",
                    Name = "Casual",
                    DisplayName = "الإجازة العارضة",
                    MaxDays = 2,
                    RequiresApproval = false,
                    AutoRenewDate = "07-01",
                    SalaryPercentage = 100,
                    IsCasual = true
                },
                new LeaveType
                {
                    Id = "leave-sick",
                    Name = "Sick",
                    DisplayName = "الإجازة المرضية",
                    MaxDays = 180,
                    RequiresApproval = true,
                    SalaryPercentage = 100,
                    IsCasual = false
                },
                new LeaveType
                {
                    Id = "leave-maternity",
                    Name = "Maternity",
                    DisplayName = "إجازة الوضع",
                    MaxDays = 120,
                    RequiresApproval = true,
                    IsGenderSpecific = true,
                    SalaryPercentage = 100,
                    IsCasual = false
                }
            );
        }
    }
 }
