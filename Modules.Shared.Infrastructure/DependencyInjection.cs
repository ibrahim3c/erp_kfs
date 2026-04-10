using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Infrastructure.Presistance.Repositories;


namespace Modules.Shared.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // مفيش DbContext هنا خالص
            // الـ Shared بس بيسجل حاجات مشتركة فعلاً
            // زي: Caching, Email, Logging, etc...

            // add services
            services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            services.AddScoped<ICityCenterRepository, CityCenterRepository>();
            return services;
        }
    }
}
