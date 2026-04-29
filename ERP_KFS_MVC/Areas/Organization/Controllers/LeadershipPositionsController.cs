using ERP_KFS_MVC.Models;
using HR.Application.Employees.AssignLeadershipPosition;
using HR.Application.Employees.GetAllEmployees;
using HR.Application.Employees.RemoveLeadershipPosition;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.JobTitle;
using Organization.Application.Dtos.LeadershipPosition;
using Organization.Application.Dtos.OrgUnit;
using Organization.Application.IServices;
using System.Diagnostics;

namespace ERP_KFS_MVC.Areas.Organization.Controllers
{
    [Area("Organization")]
    public class LeadershipPositionsController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IMediator mediator;

        public LeadershipPositionsController(IOrganizationService organizationService,IMediator mediator)
        {
            _organizationService = organizationService;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllLeadershipPositionsAsync();

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            return View(result.Value ?? new List<LeadershipPositionDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _organizationService.GetLeadershipPositionByIdAsync(id);

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

        public async Task<IActionResult> Create()
        {
            var orgResult = await _organizationService.GetAllOrgUnitsAsync();
            var jobTitles = await _organizationService.GetAllJobTitlesAsync();

            if (orgResult.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = orgResult.Error.Code,
                    ErrorMessage = orgResult.Error.Name
                });
            }

            ViewBag.OrgUnits = orgResult.Value ?? new List<OrgUnitDto>();
            ViewBag.JobTitles = jobTitles.Value ?? new List<JobTitleDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLeadershipPositionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var orgResult = await _organizationService.GetAllOrgUnitsAsync();

                if (orgResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = orgResult.Error.Code,
                        ErrorMessage = orgResult.Error.Name
                    });
                }

                ViewBag.OrgUnits = (await _organizationService.GetAllOrgUnitsAsync()).Value ?? new List<OrgUnitDto>();
                ViewBag.JobTitles = (await _organizationService.GetAllJobTitlesAsync()).Value ?? new List<JobTitleDto>();
                return View(dto);
            }

            var result = await _organizationService.CreateLeadershipPositionAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم إنشاء المنصب القيادي بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _organizationService.GetLeadershipPositionByIdAsync(id);
            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            var orgResult = await _organizationService.GetAllOrgUnitsAsync();
            if (orgResult.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = orgResult.Error.Code,
                    ErrorMessage = orgResult.Error.Name
                });

            //  ضيف JobTitles
            var jobTitlesResult = await _organizationService.GetAllJobTitlesAsync();
            if (jobTitlesResult.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = jobTitlesResult.Error.Code,
                    ErrorMessage = jobTitlesResult.Error.Name
                });

            var dto = new UpdateLeadershipPositionDto(
                result.Value.Id,
                result.Value.OrgUnitId,
                result.Value.JobTitleId,
                result.Value.Description);

            ViewBag.OrgUnits = orgResult.Value ?? new List<OrgUnitDto>();
            ViewBag.JobTitles = jobTitlesResult.Value ?? new List<JobTitleDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateLeadershipPositionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var orgResult = await _organizationService.GetAllOrgUnitsAsync();

                if (orgResult.IsFailure)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorCode = orgResult.Error.Code,
                        ErrorMessage = orgResult.Error.Name
                    });
                }

                ViewBag.OrgUnits = (await _organizationService.GetAllOrgUnitsAsync()).Value ?? new List<OrgUnitDto>();
                ViewBag.JobTitles = (await _organizationService.GetAllJobTitlesAsync()).Value ?? new List<JobTitleDto>();
                return View(dto);
            }

            var result = await _organizationService.UpdateLeadershipPositionAsync(dto);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم تحديث المنصب القيادي بنجاح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _organizationService.DeleteLeadershipPositionAsync(id);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["Success"] = "تم حذف المنصب القيادي بنجاح.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> AssignPosition()
        {
            await PopulateAssignPositionDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPosition(AssignPositionDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAssignPositionDropdownsAsync();
                return View(dto);
            }

            var command = new AssignLeadershipPositionCommand(dto.EmployeeId, dto.LeadershipPositionId);
            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });
            }

            TempData["SuccessMessage"] = "تم تعيين الموظف في المنصب القيادي بنجاح.";
            // التعديل هنا: الرجوع لنفس الصفحة عشان الجدول يتعمله تحديث
            return RedirectToAction(nameof(AssignPosition));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePosition(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                TempData["ErrorMessage"] = "رقم الموظف غير صحيح.";
                return RedirectToAction(nameof(AssignPosition)); // التعديل هنا
            }

            var result = await mediator.Send(new RemoveLeadershipPositionCommand(employeeId));

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error.Name;
                return RedirectToAction(nameof(AssignPosition)); // التعديل هنا
            }

            TempData["SuccessMessage"] = "تم إعفاء الموظف من المنصب القيادي بنجاح.";
            return RedirectToAction(nameof(AssignPosition)); // التعديل هنا
        }

        private async Task PopulateAssignPositionDropdownsAsync()
        {
            // 1. جلب كل الموظفين
            var employeesResult = await mediator.Send(new GetAllEmployeesQuery());
            var allEmployees = employeesResult.Value ?? new List<EmployeeListResponse>();

            // 2. جلب كل المناصب القيادية
            var positionsResult = await _organizationService.GetAllLeadershipPositionsAsync();
            var allPositions = positionsResult.Value ?? new List<LeadershipPositionDto>();

            var formattedPositions = allPositions.Select(p => new
            {
                Id = p.Id,
                Name = $"{p.JobTitleName} - {p.OrgUnitName}"
            }).ToList();

            ViewBag.Employees = allEmployees;
            ViewBag.Positions = formattedPositions;

            // 3. تجهيز بيانات الجدول (التعديل الصارم هنا 🔴)
            var assignedEmployees = allEmployees
                // نتأكد إن الـ ID مش بـ null وكمان مش Guid.Empty (بتاع الداتا الوهمية)
                .Where(e => e.LeadershipPositionId != null && e.LeadershipPositionId != Guid.Empty)
                .Select(e => new
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.Name,
                    // نجيب اسم المنصب لو موجود
                    PositionName = formattedPositions.FirstOrDefault(p => p.Id == e.LeadershipPositionId)?.Name
                })
                // 🔴 الفلتر الأهم: لو المنصب اتحذف أو مش موجود، شيل الموظف من الجدول فوراً
                .Where(x => !string.IsNullOrEmpty(x.PositionName))
                .ToList();

            ViewBag.AssignedEmployees = assignedEmployees;
        }
    }
}