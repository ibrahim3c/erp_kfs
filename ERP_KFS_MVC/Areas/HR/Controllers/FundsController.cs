using HR.Application.Employees.GetAllEmployees;
using HR.Application.Funds.CreateFundClaim;
using HR.Application.Funds.CreateFundSubscription;
using HR.Application.Funds.GetFundClaims;
using HR.Application.Funds.GetFundStats;
using HR.Application.Funds.GetFundSubscriptions;
using HR.Domain.Funds;
using ERP_KFS_MVC.Areas.HR.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class FundsController : Controller
    {
        private readonly IMediator _mediator;

        public FundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Fellowship()
        {
            var statsResult = await _mediator.Send(new GetFundStatsQuery());
            var subscriptionsResult = await _mediator.Send(new GetFundSubscriptionsQuery());
            var claimsResult = await _mediator.Send(new GetFundClaimsQuery());
            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());

            var model = new FundPageViewModel
            {
                Stats = statsResult.IsSuccess ? statsResult.Value : new GetFundStatsResponse(),
                Subscriptions = subscriptionsResult.IsSuccess ? subscriptionsResult.Value : new List<GetFundSubscriptionsResponse>(),
                Claims = claimsResult.IsSuccess ? claimsResult.Value : new List<GetFundClaimsResponse>(),
                Employees = employeesResult.IsSuccess ? employeesResult.Value : Enumerable.Empty<EmployeeListResponse>()
            };

            ViewBag.Employees = model.Employees;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(
            Guid employeeId,
            DateTime subscriptionDate,
            bool isFellowship,
            bool isSolidarity,
            bool bankAgreement)
        {
            FundType fundType = (isFellowship, isSolidarity) switch
            {
                (true, true) => FundType.Both,
                (true, false) => FundType.Fellowship,
                (false, true) => FundType.SocialSolidarity,
                _ => FundType.Fellowship
            };

            var command = new CreateFundSubscriptionCommand(
                EmployeeId: employeeId,
                SubscriptionDate: subscriptionDate,
                FundType: fundType,
                DeductionAmount: 0,
                BankAgreement: bankAgreement,
                Notes: null);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل الاشتراك بنجاح.";

            return RedirectToAction(nameof(Fellowship));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClaim(
            Guid employeeId,
            string claimType,
            DateTime eventDate,
            decimal? amount,
            IFormFile? attachment)
        {
            string? filePath = null;
            if (attachment is { Length: > 0 })
            {
                var folder = Path.Combine("wwwroot", "uploads", "fund-claims");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                filePath = Path.Combine("uploads", "fund-claims", fileName);
            }

            if (!Enum.TryParse<FundClaimType>(claimType, out var parsedClaimType))
            {
                TempData["ErrorMessage"] = "نوع المطالبة غير صحيح.";
                return RedirectToAction(nameof(Fellowship));
            }

            var command = new CreateFundClaimCommand(
                EmployeeId: employeeId,
                ClaimType: parsedClaimType,
                EventDate: eventDate,
                Amount: amount,
                AttachmentPath: filePath);

            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تسجيل المطالبة بنجاح.";

            return RedirectToAction(nameof(Fellowship));
        }
    }
}
