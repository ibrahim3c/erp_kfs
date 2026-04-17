using HR.Application.Payrolls.CalculatePayrollCycle;
using HR.Domain;
using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Incentives;
using HR.Domain.JobStructures;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Terminations;
using HR.Infrastructure.Persistance;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;
using HR.Infrastructure.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace HR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHRInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. تسجيل DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection")
               ?? throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<HRDbContext>(options =>
                options.UseNpgsql(connectionString));


            // 2. تسجيل الـ Repositories
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDecisionRepository, DecisionRepository>();
            services.AddScoped<IAcademicIncentiveRepository, IncentiveRepository>();
            services.AddScoped<ITerminationRepository, TerminationRepository>();
            services.AddScoped<IJobStructureRepository, JobStructureRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IInsurancePurchaseRepository, InsurancePurchaseRepository>();
            services.AddScoped<IPenaltyRepository, PenaltyRepository>();


            // 3. تسجيل الـ UnitOfWork
             services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();

            // تسجيل services in application layer
            services.AddScoped<IPayrollCalculationService, PayrollCalculationService>();

            return services;
        }
    }
}
