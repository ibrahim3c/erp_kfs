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
            
            return services;
        }
    }
}
