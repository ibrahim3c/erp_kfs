using FluentValidation;
using HR.Application.Payrolls.CalculatePayrollCycle;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHRApplication(this IServiceCollection services)
        {
            // تسجيل services in application layer
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            // تسجيل الـ FluentValidation الخاص بالـ HR
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

            return services;
        }
    }
}
