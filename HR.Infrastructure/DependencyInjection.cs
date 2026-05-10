using HR.Application.Payrolls.CalculatePayrollCycle;
using HR.Domain;
using HR.Domain.Attendance;
using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Incentives;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Promotions.Services;
using HR.Domain.Terminations;
using HR.Infrastructure.Persistance;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;
using HR.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHRInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
               ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<HRDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDecisionRepository, DecisionRepository>();
            services.AddScoped<IAcademicIncentiveRepository, IncentiveRepository>();
            services.AddScoped<ITerminationRepository, TerminationRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IInsurancePurchaseRepository, InsurancePurchaseRepository>();
            services.AddScoped<IPenaltyRepository, PenaltyRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ILateEntryRepository, LateEntryRepository>();
            services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
            services.AddScoped<IKpiReportRepository, KpiReportRepository>();
            services.AddScoped<IPromotionCycleRepository, PromotionCycleRepository>();
            services.AddScoped<IPromotionHistoryRepository, PromotionHistoryRepository>();

            services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();
            services.AddScoped<IPayrollCalculationService, PayrollCalculationService>();

            // Domain Services
            services.AddScoped<EligibilityEngine>();

            return services;
        }
    }
}
