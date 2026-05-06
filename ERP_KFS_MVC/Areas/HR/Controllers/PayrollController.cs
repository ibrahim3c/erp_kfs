using HR.Application.Employees.EmploymentTypes;
using HR.Application.Payrolls.AddPayrollAdjustment;
using HR.Application.Payrolls.CalculatePayrollCycle;
using HR.Application.Payrolls.GetPayrollCycle;
using HR.Application.Payrolls.GetPayslip;
using HR.Application.Payrolls.LockPayrollCycle;
using HR.Domain.Payrolls;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class PayrollController : Controller
    {
        // صفحة إعداد المسير
        private readonly IMediator _mediator;

        public PayrollController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  الصفحة الرئيسية — عرض المسير
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Generate(int month = 0, int year = 0)
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            var result = await _mediator.Send(new GetPayrollCycleQuery(month, year));

            // جيب EmploymentTypes عشان الـ dropdown
            var employmentTypes = await _mediator.Send(new GetAllEmploymentTypesQuery());
            ViewBag.EmploymentTypeId = employmentTypes.IsSuccess
                ? employmentTypes.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;

            return View(result.IsSuccess ? result.Value : null);
        }

        // ══════════════════════════════════════════════════════
        //  حساب الرواتب
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateCycle(
            int month, int year, Guid? employmentTypeId)
        {
            var command = new CalculatePayrollCycleCommand(month, year, employmentTypeId);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure
                    ? result.Error.Name
                    : $"تم حساب رواتب شهر {month}/{year} بنجاح.";

            return RedirectToAction(nameof(Generate), new { month, year });
        }

        // ══════════════════════════════════════════════════════
        //  إقفال الشهر
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockCycle(Guid cycleId, int month, int year)
        {
            var result = await _mediator.Send(new LockPayrollCycleCommand(cycleId));

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم إقفال المسير بنجاح.";

            return RedirectToAction(nameof(Generate), new { month, year });
        }

        // ══════════════════════════════════════════════════════
        //  تسوية يدوية (إضافة / خصم)
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdjustment(
            Guid entryId,
            string adjType,
            decimal amount,
            string reason,
            int month,
            int year)
        {
            var type = adjType == "plus"
                ? AdjustmentType.Addition
                : AdjustmentType.Deduction;

            var command = new AddPayrollAdjustmentCommand(entryId, type, amount, reason);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تحديث صافي الراتب بنجاح.";

            return RedirectToAction(nameof(Generate), new { month, year });
        }

        // ══════════════════════════════════════════════════════
        //  مفردات مرتب موظف
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Payslip(Guid entryId)
        {
            if (entryId == Guid.Empty)
                return RedirectToAction(nameof(Generate));

            var result = await _mediator.Send(new GetPayslipQuery(entryId));

            if (result.IsFailure || result.Value is null)
                return RedirectToAction(nameof(Generate));

            return View(result.Value);
        }
    }
}