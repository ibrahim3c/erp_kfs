using HR.Domain.Abstractions;
using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Organization;
using HR.Infrastructure.Persistance;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Infrastructure.Presistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
            services.AddScoped<IOrgUnitTypeRepository, OrgUnitTypeRepository>();
           


            // 3. تسجيل الـ UnitOfWork
            services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();

            return services;
        }
    }
}
