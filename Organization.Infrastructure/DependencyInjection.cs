using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application.IServices;
using Organization.Domain;
using Organization.Domain.IRepositories;
using Organization.Infrastructure.Database;
using Organization.Infrastructure.Repositories;
using Organization.Infrastructure.Services;

namespace Organization.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrganizationInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<OrganizationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IOrgUnitTypeRepository, OrgUnitTypeRepository>();
            services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
            services.AddScoped<ILeadershipPositionRepository, LeadershipPositionRepository>();
            services.AddScoped<ILeadershipPositionHistoryRepository, LeadershipPositionHistoryRepository>();
            services.AddScoped<IQualitativeGroupRepository, QualitativeGroupRepository>();
            services.AddScoped<IFunctionalGroupRepository, FunctionalGroupRepository>();
            services.AddScoped<IJobTitleRepository, JobTitleRepository>();
            services.AddScoped<IJobGradeRepository, JobGradeRepository>();

            services.AddScoped<IOrganizationUnitOfWork, OrganizationUnitOfWork>();

            services.AddScoped<IOrganizationService, OrganizationService>();

            return services;
        }
    }
}