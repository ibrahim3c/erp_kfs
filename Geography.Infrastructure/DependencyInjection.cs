using Geography.Application.IServices;
using Geography.Domain;
using Geography.Domain.IRepositories;
using Geography.Domain.Repositories;
using Geography.Infrastructure.Database;
using Geography.Infrastructure.Repositories;
using Geography.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Geography.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGeographyInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("==========================================");
            Console.WriteLine($"Connection String: {connectionString}");
            Console.WriteLine("==========================================");

                if(connectionString is null )
                    throw new InvalidOperationException($"Connection string '{connectionString}' not found.");


            services.AddDbContext<GeographyDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            services.AddScoped<ICityCenterRepository, CityCenterRepository>();
            services.AddScoped<ILocalunitRepository, LocalunitRepository>();
            services.AddScoped<IVillageRepository, VillageRepository>();

            services.AddScoped<IGeographyUnitOfWork, GeographyUnitOfWork>();

            services.AddScoped<IGeographyService, GeographyService>();

            return services;
        }
    }
}