using HR.Application.JobStructures;
using HR.Application.JobStructures.CreateJobGrade;
using HR.Application.JobStructures.CreateJobTitle;
using HR.Application.JobStructures.GetJobGradeList;
using HR.Application.JobStructures.GetJobTitleList;
using HR.Application.JobStructures.GetQualitativeGroupList;
using HR.Application.JobStructures.UpdateJobTitle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyERP.Web.Areas.HR.Controllers
{
    [Area("HR")]
    public class LookupsController : Controller
    {
        private readonly IMediator _mediator;

        public LookupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ══════════════════════════════════════════════════════
        //  الصفحة الرئيسية — بتجيب كل البيانات دفعة واحدة
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var gradesResult = await _mediator.Send(new GetJobGradeListQuery());
            var titlesResult = await _mediator.Send(new GetJobTitleListQuery());
            var qualGroupsResult = await _mediator.Send(new GetQualitativeGroupListQuery());

            if (gradesResult.IsFailure) return View("Error", gradesResult.Error);
            if (titlesResult.IsFailure) return View("Error", titlesResult.Error);
            if (qualGroupsResult.IsFailure) return View("Error", qualGroupsResult.Error);

            var viewModel = new JobStructureViewModel
            {
                JobGrades = gradesResult.Value!,
                JobTitles = titlesResult.Value!,
                QualitativeGroups = qualGroupsResult.Value!
            };

            return View(viewModel);
        }

        // ══════════════════════════════════════════════════════
        //  JobGrade — إضافة درجة
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJobGrade(
            string code,
            string name,
            int gradeLevel,
            string description,
            int yearsNo)
        {
            var command = new CreateJobGradeCommand(code, name, gradeLevel, description, yearsNo);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم إضافة الدرجة المالية بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        //  JobTitle — إضافة مسمى وظيفي
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJobTitle(
            Guid functionalGroupId,
            string code,
            string name,
            string description)
        {
            var command = new CreateJobTitleCommand(functionalGroupId, code, name, description);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم إضافة المسمى الوظيفي بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════
        //  JobTitle — تعديل مسمى وظيفي
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJobTitle(
            Guid id,
            string code,
            string name,
            string description)
        {
            if (id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "رقم المسمى الوظيفي غير صحيح.";
                return RedirectToAction(nameof(Index));
            }

            var command = new UpdateJobTitleCommand(id, code, name, description);
            var result = await _mediator.Send(command);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم تعديل المسمى الوظيفي بنجاح.";

            return RedirectToAction(nameof(Index));
        }
    }
}