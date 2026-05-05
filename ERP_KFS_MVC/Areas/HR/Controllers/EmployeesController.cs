using ERP_KFS_MVC.Areas.HR.ViewModels;
using ERP_KFS_MVC.Models;
using Geography.Application.IServices;
using HR.Application.Employees.CreateFullEmployee;
using HR.Application.Employees.DeleteEmployee;
using HR.Application.Employees.EmploymentTypes;
using HR.Application.Employees.GetAllEmployeeActiveAndNot;
using HR.Application.Employees.GetAllEmployees;
using HR.Application.Employees.GetAllQualificationTypes;
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
            ViewBag.JobTitleId = jobTitles.IsSuccess
                ? jobTitles.Value.Select(x => new SelectListItem
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

            var qualificationTypes = await _mediator.Send(new GetAllQualificationTypesQuery());
            ViewBag.QualificationTypeId = qualificationTypes.IsSuccess
                ? qualificationTypes.Value.Select(x => new SelectListItem
                {
                    Value = x.id.ToString(),
                    Text = x.name
                }).ToList()
                : new List<SelectListItem>();

        }

        // ─────────────────────────────────────────
        // GET: /HR/Employees/Index
        // ─────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllEmployeesActiveAndNotQuery());

            if (result.IsFailure)
                return View("Error", new ErrorViewModel
                {
                    ErrorCode = result.Error.Code,
                    ErrorMessage = result.Error.Name
                });

            return View(result.Value ?? new List<GetAllEmployeesQueryActiveAndNotResponse>());
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
            return View(new CreateFullEmployeeViewModel());
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

            var command = new CreateFullEmployeeCommand(
                // 1. Personal Information
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

                // 2. Job Information
                OrgUnitId: vm.OrgUnitId,
                JobTitleId: vm.JobTitleId ?? Guid.Empty,
                JobGradeId: vm.JobGradeId,
                EmploymentTypeId: vm.EmploymentTypeId,
                FunctionalGroupId: vm.FunctionalGroupId,
                HireDate: vm.HireDate,
                JobGradeDate: vm.JobGradeDate,

                // 3. E-Files (IFormFile passed directly)
                ProfileImagePath: vm.ProfileImage,
                NationalIdCardFrontPath: vm.NationalIdCardFront,
                NationalIdCardBackPath: vm.NationalIdCardBack,
                QualificationFilePath: vm.QualificationFile,
                BirthCertificatePath: vm.BirthCertificate,
                MilitaryFilePath: vm.MilitaryFile,
                ContractFilePath: vm.ContractFile,
                PoliceClearancePath: vm.PoliceClearance,
                MarriageDocumentPath: vm.MarriageDocument,

                // 4. Financial Information
                BasicSalary2019: vm.BasicSalary2019,
                GrossSalary: vm.GrossSalary,
                InsuranceNumber: vm.InsuranceNumber,
                BankName: vm.BankName,
                BankAccountNumber: vm.BankAccountNumber,
                HasFellowshipFund: vm.HasFellowshipFund,
                HasMedicalFund: vm.HasMedicalFund,

                // 5. Employee Qualification
                QualificationTypeId: vm.QualificationTypeId,
                QualificationFullName: vm.QualificationFullName,
                Specialization: vm.Specialization,
                University: vm.University,
                GraduationYear: vm.GraduationYear,
                Grade: vm.Grade,
                QualificationValidFrom: vm.ValidFrom,
                QualificationValidTo: vm.ValidTo,
                QualificationNotes: vm.Notes
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
                Email = r.Email,
                Address = r.Address,
                MaritalStatus = r.MaritalStatus,
                IsActive = r.IsActive,
                IsDisabled = r.IsDisabled,
                HireDate = r.HireDate,
                JobGradeDate = r.JobGradeDate,
                OrgUnitId = r.OrgUnitId,
                JobTitleId = r.JobTitleId,
                JobGradeId = r.JobGradeId,
                EmploymentTypeId = r.EmploymentTypeId,
                FunctionalGroupId = r.FunctionalGroupId,
                JobTitleName = r.JobTitleName,
                QualificationName = r.QualificationTypeName,
                GrossSalary = r.GrossSalary,
                BasicSalary2019 = r.BasicSalary2019,
                InsuranceNumber = r.InsuranceNumber,
                BankName = r.BankName,
                BankAccountNumber = r.BankAccountNumber,
                HasFellowshipFund = r.HasFellowshipFund,
                HasMedicalFund = r.HasMedicalFund,

                // الملفات الحالية
                CurrentPersonalPhoto = r.PersonalPhoto,
                CurrentNationalIdCardFront = r.NationalIdCardFront,
                CurrentNationalIdCardBack = r.NationalIdCardBack,
                CurrentQualificationFile = r.QualificationFile,
                CurrentBirthCertificateFile = r.BirthCertificateFile,
                CurrentMilitaryFile = r.MilitaryFile,
                CurrentContractFile = r.ContractFile,
                CurrentPoliceClearance = r.PoliceClearanceCertificate,
                CurrentMarriageDocument = r.MarriageDocument,
            };

            await PopulateDropdownsAsync();
            return View(vm);
        }

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
                Email: vm.Email,
                Gender: vm.Gender,
                Address: vm.Address,
                MaritalStatus: vm.MaritalStatus,
                DateOfBirth: vm.DateOfBirth,
                HireDate: vm.HireDate,
                JobGradeDate: vm.JobGradeDate,
                IsDisabled: vm.IsDisabled,
                OrgUnitId: vm.OrgUnitId,
                JobTitleId: vm.JobTitleId,
                JobGradeId: vm.JobGradeId,
                EmploymentTypeId: vm.EmploymentTypeId,
                FunctionalGroupId: vm.FunctionalGroupId,
                GrossSalary: vm.GrossSalary,
                BasicSalary2019: vm.BasicSalary2019,
                InsuranceNumber: vm.InsuranceNumber,
                BankName: vm.BankName,
                BankAccountNumber: vm.BankAccountNumber,
                HasFellowshipFund: vm.HasFellowshipFund,
                HasMedicalFund: vm.HasMedicalFund,
                ProfileImage: vm.ProfileImage,
                NationalIdCardFront: vm.NationalIdCardFront,
                NationalIdCardBack: vm.NationalIdCardBack,
                QualificationFile: vm.QualificationFile,
                BirthCertificate: vm.BirthCertificate,
                MilitaryFile: vm.MilitaryFile,
                ContractFile: vm.ContractFile,
                PoliceClearance: vm.PoliceClearance,
                MarriageDocument: vm.MarriageDocument,
                CurrentPersonalPhoto: vm.CurrentPersonalPhoto,
                CurrentNationalIdCardFront: vm.CurrentNationalIdCardFront,
                CurrentNationalIdCardBack: vm.CurrentNationalIdCardBack,
                CurrentQualificationFile: vm.CurrentQualificationFile,
                CurrentBirthCertificateFile: vm.CurrentBirthCertificateFile,
                CurrentMilitaryFile: vm.CurrentMilitaryFile,
                CurrentContractFile: vm.CurrentContractFile,
                CurrentPoliceClearance: vm.CurrentPoliceClearance,
                CurrentMarriageDocument: vm.CurrentMarriageDocument
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