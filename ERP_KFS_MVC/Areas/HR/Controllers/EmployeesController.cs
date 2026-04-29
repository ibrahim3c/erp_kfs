using ERP_KFS_MVC.Models;
using Geography.Application.IServices;
using HR.Application.Employees.CreateEmployee;
using HR.Application.Employees.DeleteEmployee;
using HR.Application.Employees.EmploymentTypes;
using HR.Application.Employees.GetAllEmployees;
using HR.Application.Employees.GetEmployeeDetails;
using HR.Application.Employees.GetEmployeeForDelete;
using HR.Application.Employees.GetEmployeeForEdit;
using HR.Application.Employees.UpdateEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Organization.Application.IServices;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class EmployeesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IGeographyService _geographyService;
        private readonly IOrganizationService _organizationService;

        public EmployeesController(
            IMediator mediator,
            IGeographyService geographyService,
            IOrganizationService organizationService)
        {
            _mediator = mediator;
            _geographyService = geographyService;
            _organizationService = organizationService;
        }

        // ─────────────────────────────────────────
        // Helper: تملي الـ ViewBag بكل الـ dropdowns
        // ─────────────────────────────────────────
        private async Task PopulateDropdownsAsync()
        {
            // Geography
            var cityCenters = await _geographyService.GetAllCityCentersAsync();
            ViewBag.CityCenterId = cityCenters.IsSuccess
                ? cityCenters.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var villages = await _geographyService.GetAllVillagesAsync();
            ViewBag.VillageId = villages.IsSuccess
                ? villages.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            // Organization
            var employmentTypes = await _mediator.Send(new GetAllEmploymentTypesQuery());
            ViewBag.EmploymentTypeId = employmentTypes.IsSuccess
                ? employmentTypes.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var jobTitles = await _organizationService.GetAllJobTitlesAsync();
            ViewBag.JobTitleId = jobTitles.IsSuccess
                ? jobTitles.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var jobGrades = await _organizationService.GetAllJobGradesAsync();
            ViewBag.JobGradeId = jobGrades.IsSuccess
                ? jobGrades.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var functionalGroups = await _organizationService.GetAllFunctionalGroupsAsync();
            ViewBag.FunctionalGroupId = functionalGroups.IsSuccess
                ? functionalGroups.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var orgUnits = await _organizationService.GetAllOrgUnitsAsync();
            ViewBag.OrgUnitId = orgUnits.IsSuccess
                ? orgUnits.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery());

            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            return View(result.Value ?? new List<EmployeeListResponse>());
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Details/{id}
        // ─────────────────────────────────────────
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeDetailsQuery(id));

            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            return View(result.Value);
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Create
        // ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // ─────────────────────────────────────────
        // POST: /HR/Employees/Create
        // ─────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(command);
            }

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error.Name);
                await PopulateDropdownsAsync();
                return View(command);
            }

            TempData["Success"] = "تم إضافة الموظف بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Edit/{id}
        // ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeForEditQuery(id));

            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            var command = new UpdateEmployeeCommand(
                Id: result.Value!.Id,
                Name: result.Value.Name,
                Code: result.Value.Code,
                Phone: result.Value.Phone,
                Email: result.Value.Email,
                HireDate: result.Value.HireDate,
                IsActive: result.Value.IsActive,
                CreatedAt: result.Value.CreatedAt
            );

            await PopulateDropdownsAsync();
            return View(command);
        }

        // ─────────────────────────────────────────
        // POST: /HR/Employees/Edit/{id}
        // ─────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(command);
            }

            var finalCommand = command with { Id = id };
            var result = await _mediator.Send(finalCommand);

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error.Name);
                await PopulateDropdownsAsync();
                return View(command);
            }

            TempData["Success"] = "تم تحديث بيانات الموظف بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Delete/{id}
        // ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeForDeleteQuery(id));

            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            return View(result.Value);
        }

        // ─────────────────────────────────────────
        // POST: /HR/Employees/Delete/{id}
        // ─────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));

            if (result.IsFailure)
            {
                TempData["Error"] = result.Error.Name;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "تم حذف الموظف بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}