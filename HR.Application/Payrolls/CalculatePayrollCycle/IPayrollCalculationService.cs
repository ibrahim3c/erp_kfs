using HR.Domain.Employees;
using HR.Domain.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Payrolls.CalculatePayrollCycle
{
    /// <summary>
    /// خدمة حساب الرواتب — بتجمع كل المكونات من كل الموديولات
    /// </summary>
    public interface IPayrollCalculationService
    {
        Task<List<PayrollEntry>> CalculateAsync(
            int month,
            int year,
            EmploymentType employeeCategory,
            Guid cycleId,
            CancellationToken cancellationToken = default);
    }
}
