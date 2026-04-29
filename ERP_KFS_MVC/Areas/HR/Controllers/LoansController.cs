using ERP_KFS_MVC.Models;
using HR.Application.Loans;
using HR.Application.Loans.ApproveInsurancePurchase;
using HR.Application.Loans.CreateInsurancePurchase;
using HR.Application.Loans.CreateLoan;
using HR.Application.Loans.GetInsurancePurchaseList;
using HR.Application.Loans.GetLoanDetails;
using HR.Application.Loans.GetLoanList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class LoansController : Controller
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  صفحة رئيسية — السلف الشخصية + شراء المدد
        // ══════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var loansResult = await _mediator.Send(new GetLoanListQuery());

            if (loansResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = loansResult.Error.Code,
                    ErrorMessage = loansResult.Error.Name
                });
            }

            var insuranceResult = await _mediator.Send(new GetInsurancePurchaseListQuery());

            if (insuranceResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = insuranceResult.Error.Code, // تم تصحيح الخطأ هنا
                    ErrorMessage = insuranceResult.Error.Name
                });
            }

            var viewModel = new LoanRequestsViewModel
            {
                Loans = loansResult.Value!,
                InsurancePurchases = insuranceResult.Value!
            };

            return View(viewModel);
        }

        // ══════════════════════════════════════════════════════
        //  السلف الشخصية
        // ══════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "رقم السلفة غير صحيح.";
                return RedirectToAction(nameof(Requests));
            }

            var result = await _mediator.Send(new GetLoanDetailsQuery(id));

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan(
            Guid employeeId,
            decimal amount,
            int months,
            string startMonth,   // من <input type="month"> — مثال: "2024-05"
            string reason)
        {
            if (!DateTime.TryParseExact(startMonth, "yyyy-MM",
                    null, System.Globalization.DateTimeStyles.None, out var startDate))
            {
                TempData["ErrorMessage"] = "تاريخ البداية غير صحيح.";
                return RedirectToAction(nameof(Requests));
            }

            var command = new CreateLoanCommand(employeeId, amount, months, startDate, reason);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["SuccessMessage"] = "تم صرف السلفة بنجاح وجدولة الأقساط.";
            return RedirectToAction(nameof(Requests));
        }

        // ══════════════════════════════════════════════════════
        //  شراء المدد التأمينية
        // ══════════════════════════════════════════════════════

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInsurancePurchase(
            Guid employeeId,
            string insuranceAuthority,
            int purchasedYears,
            decimal totalCost,
            decimal monthlyInstallment,
            string deductionStartMonth,   // من <input type="month">
            IFormFile? approvalDecisionFile)
        {
            if (!DateTime.TryParseExact(deductionStartMonth, "yyyy-MM",
                    null, System.Globalization.DateTimeStyles.None, out var deductionStartDate))
            {
                TempData["ErrorMessage"] = "تاريخ بداية الخصم غير صحيح.";
                return RedirectToAction(nameof(Requests));
            }

            // رفع صورة القرار لو موجودة
            string? filePath = null;
            if (approvalDecisionFile is { Length: > 0 })
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "insurance-decisions");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(approvalDecisionFile.FileName)}";
                filePath = Path.Combine(uploadsFolder, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await approvalDecisionFile.CopyToAsync(stream);

                // نخزن المسار النسبي بس
                filePath = Path.Combine("uploads", "insurance-decisions", fileName);
            }

            var command = new CreateInsurancePurchaseCommand(
                employeeId,
                insuranceAuthority,
                purchasedYears,
                totalCost,
                monthlyInstallment,
                deductionStartDate,
                filePath);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["SuccessMessage"] = "تم تسجيل طلب شراء المدة التأمينية بنجاح.";
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveInsurancePurchase(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "رقم الطلب غير صحيح.";
                return RedirectToAction(nameof(Requests));
            }

            var result = await _mediator.Send(new ApproveInsurancePurchaseCommand(id));

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["SuccessMessage"] = "تم اعتماد الطلب بنجاح.";
            return RedirectToAction(nameof(Requests));
        }
    }
}