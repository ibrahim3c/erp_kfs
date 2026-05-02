using ERP_KFS_MVC.Areas.HR.ViewModels;
using ERP_KFS_MVC.Models;
using Geography.Application.IServices;
using HR.Application.Employees.CreateFullEmployee;
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
        private readonly IWebHostEnvironment _env;

        public EmployeesController(
            IMediator mediator,
            IGeographyService geographyService,
            IOrganizationService organizationService,
            IWebHostEnvironment env)
        {
            _mediator = mediator;
            _geographyService = geographyService;
            _organizationService = organizationService;
            _env = env;
        }

        // ─────────────────────────────────────────
        // Helper: تملي الـ ViewBag بكل الـ dropdowns
        // ─────────────────────────────────────────
        private async Task PopulateDropdownsAsync()
        {
            var cityCenters = await _geographyService.GetAllCityCentersAsync();
            ViewBag.CityCenterId = cityCenters.IsSuccess
                ? cityCenters.Value.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var employmentTypes = await _mediator.Send(new GetAllEmploymentTypesQuery());
            ViewBag.EmploymentTypeId = employmentTypes.IsSuccess
                ? employmentTypes.Value.Select(x => new SelectListItem
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

            var jobTitles = await _organizationService.GetAllJobTitlesAsync();
            ViewBag.JobTitleName = jobTitles.IsSuccess
                ? jobTitles.Value.Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                }).ToList()
                : new List<SelectListItem>();

            var fubctionalGroups = await _organizationService.GetAllFunctionalGroupsAsync();
                ViewBag.FunctionalGroupName = fubctionalGroups.IsSuccess
                    ? fubctionalGroups.Value.Select(x => new SelectListItem
                    {
                        Value = x.Name,
                        Text = x.Name
                    }).ToList() : new List<SelectListItem>();

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
        // Helper: رفع ملف وإرجاع المسار
        // ─────────────────────────────────────────
        private async Task<string?> SaveFileAsync(IFormFile? file, string subFolder)
        {
            if (file is null || file.Length == 0) return null;

            // wwwroot/uploads/hr/{subFolder}/
            var folder = Path.Combine(_env.WebRootPath, "uploads", "hr", subFolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // نرجع المسار النسبي — سهل للتخزين في الـ DB
            return $"/uploads/hr/{subFolder}/{fileName}";
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
        public async Task<IActionResult> Create(CreateFullEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(vm);
            }

            var profileImagePath = await SaveFileAsync(vm.ProfileImage, "profile");
            var nationalIdCardPath = await SaveFileAsync(vm.NationalIdCard, "national-id");
            var qualificationFilePath = await SaveFileAsync(vm.QualificationFile, "qualification");
            var birthCertificatePath = await SaveFileAsync(vm.BirthCertificate, "birth-cert");
            var militaryFilePath = await SaveFileAsync(vm.MilitaryFile, "military");
            var contractFilePath = await SaveFileAsync(vm.ContractFile, "contract");
            var policeClearancePath = await SaveFileAsync(vm.PoliceClearance, "police");

            var command = new CreateFullEmployeeCommand(
                FirstName: vm.FirstName,
                FatherName: vm.FatherName,
                LastName: vm.LastName,
                NationalId: vm.NationalId,
                DateOfBirth: vm.DateOfBirth,
                Gender: vm.Gender,
                Phone: vm.Phone,
                 Email: vm.Email,
                 MaritalStatus: vm.MaritalStatus,
                Address: vm.Address,
                IsDisabled: vm.IsDisabled,
                OrgUnitId: vm.OrgUnitId,
                JobTitleName: vm.JobTitleName,
                JobTitleId: vm.JobTitleId ?? Guid.Empty,
                QualificationName: vm.QualificationName,
                JobGradeId: vm.JobGradeId,
                HireDate: vm.HireDate,
                JobGradeDate: vm.JobGradeDate,
                EmploymentTypeId: vm.EmploymentTypeId,
                ProfileImagePath: profileImagePath,
                NationalIdCardPath: nationalIdCardPath,
                QualificationFilePath: qualificationFilePath,
                BirthCertificatePath: birthCertificatePath,
                MilitaryFilePath: militaryFilePath,
                ContractFilePath: contractFilePath,
                PoliceClearancePath: policeClearancePath,
                BasicSalary2019: vm.BasicSalary2019,
                GrossSalary: vm.GrossSalary,
                InsuranceNumber: vm.InsuranceNumber,
                BankName: vm.BankName,
                BankAccountNumber: vm.BankAccountNumber,
                HasFellowshipFund: vm.HasFellowshipFund,
                HasMedicalFund: vm.HasMedicalFund
            );

     
   
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {

                ModelState.AddModelError(string.Empty, result.Error.Name);

                await PopulateDropdownsAsync();
                return View(vm);
            }

                TempData["Success"] = "تم إضافة الموظف بنجاح";
                return RedirectToAction(nameof(Index));          
    
        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Edit/{id}
        // ─────────────────────────────────────────
        // في EmployeesController — Edit GET معدّل
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

            var r = result.Value!;

            var vm = new UpdateEmployeeViewModel
            {
                Id = r.Id,
                Code = r.Code,
                FirstName = r.Name.Split(' ').ElementAtOrDefault(0) ?? string.Empty,
                FatherName = r.Name.Split(' ').ElementAtOrDefault(1) ?? string.Empty,
                LastName = r.Name.Split(' ').ElementAtOrDefault(2) ?? string.Empty,
                NationalId = r.NationalId,
                DateOfBirth = r.DateOfBirth,
                Gender = r.Gender,
                Phone = r.Phone,
                Address = r.Address,
                MaritalStatus = r.MaritalStatus,
                IsActive = r.IsActive,
                IsDisabled = r.IsDisabled,
                HireDate = r.HireDate,
                JobGradeDate = r.JobGradeDate,
                OrgUnitId = r.OrgUnitId,
                JobGradeId = r.JobGradeId,
                EmploymentTypeId = r.EmploymentTypeId,
                JobTitleName = r.JobTitleName,
                QualificationName = r.QualificationName,
                GrossSalary = r.GrossSalary,
                BasicSalary2019 = r.BasicSalary2019,
                InsuranceNumber = r.InsuranceNumber,
                BankName = r.BankName,
                BankAccountNumber = r.BankAccountNumber,
                HasFellowshipFund = r.HasFellowshipFund,
                HasMedicalFund = r.HasMedicalFund,
            };

            await PopulateDropdownsAsync();
            return View(vm);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(vm);
            }

            string fullName = $"{vm.FirstName} {vm.FatherName} {vm.LastName}".Trim();

            var command = new UpdateEmployeeCommand(
                Id: id,
                Name: fullName,
                Code: vm.Code,
                Phone: vm.Phone,
                Email: null,
                HireDate: vm.HireDate,
                IsActive: vm.IsActive,
                CreatedAt: DateTime.UtcNow   // أو تجيبه من الـ response
            );

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error.Name);
                await PopulateDropdownsAsync();
                return View(vm);
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

        // ─────────────────────────────────────────
        // GET API: /HR/Employees/VillagesByCityCenter/{id}
        // ─────────────────────────────────────────
        [HttpGet("/api/geography/villages-by-city-center/{cityCenterId:guid}")]
        public async Task<IActionResult> VillagesByCityCenter(Guid cityCenterId)
        {
            var result = await _geographyService.GetVillagesByCityCenterIdAsync(cityCenterId);

            if (result.IsFailure)
                return NotFound();

            return Ok(result.Value.Select(v => new { id = v.Id, name = v.Name }));
        }
    }
}