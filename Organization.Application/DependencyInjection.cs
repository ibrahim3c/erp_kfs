using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Organization.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrganizationApplication(this IServiceCollection services)
        {
            // Implementation for adding organization infrastructure services
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            return services;
        }
    }
}
