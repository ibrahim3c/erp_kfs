using HR.Application.Departments.GetDepartmentStats;
using HR.Application.Departments.GetOrgUnitTree;
using HR.Application.Departments.GetOrgUnitTypeOptions;
using HR.Application.Employees.GetAllEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Dtos.OrgUnit;
using Organization.Application.IServices;
using ERP_KFS_MVC.Areas.HR.ViewModels;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class DepartmentsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrganizationService _organizationService;

        public DepartmentsController(IMediator mediator, IOrganizationService organizationService)
        {
            _mediator = mediator;
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var treeResult = await _mediator.Send(new GetOrgUnitTreeQuery());
            if (treeResult.IsFailure)
                return View("Error", new { ErrorCode = "Departments.Tree", ErrorMessage = treeResult.Error.Name });

            var statsResult = await _mediator.Send(new GetDepartmentStatsQuery());
            ViewBag.Stats = statsResult.IsSuccess
                ? statsResult.Value
                : new GetDepartmentStatsResponse();

            var typeOptionsResult = await _mediator.Send(new GetOrgUnitTypeOptionsQuery());
            ViewBag.OrgUnitTypes = typeOptionsResult.IsSuccess
                ? typeOptionsResult.Value
                : new List<GetOrgUnitTypeOptionsResponse>();

            var employeesResult = await _mediator.Send(new GetAllEmployeesQuery());
            ViewBag.Employees = employeesResult.IsSuccess
                ? employeesResult.Value
                : new List<EmployeeListResponse>();

            var flatList = treeResult.Value ?? new List<GetOrgUnitTreeResponse>();

            var treeNodes = BuildTree(flatList);

            ViewBag.FlatUnits = flatList;

            return View(treeNodes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(
            string name,
            string code,
            Guid orgUnitTypeId,
            Guid? parentId)
        {
            var dto = new CreateOrgUnitDto(name, code, orgUnitTypeId, parentId, null);

            var result = await _organizationService.CreateOrgUnitAsync(dto);

            TempData[result.IsFailure ? "ErrorMessage" : "SuccessMessage"] =
                result.IsFailure ? result.Error.Name : "تم إضافة الوحدة التنظيمية بنجاح.";

            return RedirectToAction(nameof(Index));
        }

        private static List<OrgUnitTreeNodeViewModel> BuildTree(
            List<GetOrgUnitTreeResponse> flatList)
        {
            var lookup = flatList.ToLookup(
                x => x.ParentId,
                x => new OrgUnitTreeNodeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    LevelOrder = x.LevelOrder,
                    CurrentManagerName = x.CurrentManagerName,
                    EmployeeCount = x.EmployeeCount
                });

            var allNodes = new Dictionary<Guid, OrgUnitTreeNodeViewModel>();
            foreach (var item in flatList)
            {
                allNodes[item.Id] = new OrgUnitTreeNodeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    LevelOrder = item.LevelOrder,
                    CurrentManagerName = item.CurrentManagerName,
                    EmployeeCount = item.EmployeeCount
                };
            }

            foreach (var node in allNodes.Values)
            {
                var children = flatList
                    .Where(x => x.ParentId == node.Id)
                    .Select(x => allNodes[x.Id])
                    .ToList();
                node.Children = children;
            }

            var roots = flatList
                .Where(x => x.ParentId == null)
                .Select(x => allNodes[x.Id])
                .ToList();

            return roots;
        }
    }
}
