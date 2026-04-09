using HR.Domain.Candidates;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;
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

            // 3. تسجيل الـ UnitOfWork
            // services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();

            return services;
        }
    }
}
