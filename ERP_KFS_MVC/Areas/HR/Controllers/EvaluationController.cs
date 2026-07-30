using HR.Application.Employees.GetAllEmployees;
using HR.Application.Evaluations.CreateGrievance;
using HR.Application.Evaluations.CreateKpiReport;
using HR.Application.Evaluations.GetGrievanceList;
using HR.Application.Evaluations.GetGrievanceStats;
using HR.Application.Evaluations.GetKpiReportList;
using HR.Application.Evaluations.GetKpiReportStats;
using HR.Application.Evaluations.ResolveGrievance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class EvaluationController : Controller
    {
        private readonly IMediator _mediator;

        public EvaluationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> AnnualReport(int? year)
        {
            var kpiQuery = new GetKpiReportListQuery(year);
            var statsQuery = new GetKpiReportStatsQuery(year);
            var employeesQuery = new GetAllEmployeesQuery();

            var kpiResult = await _mediator.Send(kpiQuery);
            var statsResult = await _mediator.Send(statsQuery);
            var employeesResult = await _mediator.Send(employeesQuery);

            ViewBag.Stats = statsResult.IsSuccess ? statsResult.Value : new GetKpiReportStatsResponse();
            ViewBag.Employees = employeesResult.IsSuccess
                ? employeesResult.Value!.Where(e => e.IsActive).ToList()
                : new List<EmployeeListResponse>();
            ViewBag.CurrentYear = year;

            return View(kpiResult.IsSuccess ? kpiResult.Value! : new List<GetKpiReportListResponse>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateKpiReport(
            Guid EmployeeId, int Year,
            decimal EfficiencyScore, decimal DisciplineScore, decimal AchievementScore,
            string? Notes)
        {
            var command = new CreateKpiReportCommand(
                EmployeeId, Year,
                EfficiencyScore, DisciplineScore, AchievementScore,
                null, "Draft", Notes);

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "تم حفظ تقرير التقويم بنجاح";
            else
                TempData["ErrorMessage"] = result.Error.Name;

            return RedirectToAction(nameof(AnnualReport), new { Year = Year });
        }

        public async Task<IActionResult> Grievances()
        {
            var listQuery = new GetGrievanceListQuery();
            var statsQuery = new GetGrievanceStatsQuery();
            var employeesQuery = new GetAllEmployeesQuery();

            var listResult = await _mediator.Send(listQuery);
            var statsResult = await _mediator.Send(statsQuery);
            var employeesResult = await _mediator.Send(employeesQuery);

            ViewBag.Stats = statsResult.IsSuccess ? statsResult.Value : new GetGrievanceStatsResponse();
            ViewBag.Employees = employeesResult.IsSuccess
                ? employeesResult.Value!.Where(e => e.IsActive).ToList()
                : new List<EmployeeListResponse>();

            return View(listResult.IsSuccess ? listResult.Value! : new List<GetGrievanceListResponse>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGrievance(
            Guid EmployeeId, string GrievanceType,
            string ComplainedDecisionNumber, DateTime ComplainedDecisionDate,
            DateTime SubmissionDate, string Reasons)
        {
            var command = new CreateGrievanceCommand(
                EmployeeId, GrievanceType,
                ComplainedDecisionNumber, ComplainedDecisionDate,
                SubmissionDate, Reasons, null);

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "تم تسجيل التظلم وإحالته للجنة بنجاح";
            else
                TempData["ErrorMessage"] = result.Error.Name;

            return RedirectToAction(nameof(Grievances));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveGrievance(
            Guid GrievanceId, string NewStatus,
            string? CommitteeNotes, DateTime ResolutionDate)
        {
            var command = new ResolveGrievanceCommand(
                GrievanceId, NewStatus, CommitteeNotes, ResolutionDate);

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "تم اعتماد قرار اللجنة بنجاح";
            else
                TempData["ErrorMessage"] = result.Error.Name;

            return RedirectToAction(nameof(Grievances));
        }
    }
}
