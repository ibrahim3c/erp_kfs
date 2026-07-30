using HR.Application.Secondments.Query.GetEmployeesForSelect;
using HR.Application.Transefers.Command.ApproveInternalTransfer;
using HR.Application.Transefers.Command.CreateExternalMovement;
using HR.Application.Transefers.Command.CreateInternalTransfer;
using HR.Application.Transefers.Command.EndExternalMovement;
using HR.Application.Transefers.Command.RenewExternalMovement;
using HR.Application.Transefers.Query.GetDepartmentsForSelect;
using HR.Application.Transefers.Query.GetExternalMovements;
using HR.Application.Transefers.Query.GetInternalTransefers;
using HR.Domain.Transfers.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class TransfersController : Controller
    {
        private readonly IMediator _mediator;
        public TransfersController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index()
        {
            var internalTransfers = await _mediator.Send(new GetInternalTransfersQuery());
            var externalMovements = await _mediator.Send(new GetExternalMovementsQuery());
            var employees = await _mediator.Send(new GetEmployeesForSelectQuery(null));
            var departments = await _mediator.Send(new GetDepartmentsForSelectQuery());

            ViewBag.Employees = employees.Value ?? new();
            ViewBag.Departments = departments.Value?.Departments ?? new();
            ViewBag.JobTitles = departments.Value?.JobTitles ?? new();
            ViewBag.InternalTransfers = internalTransfers.Value ?? new();
            ViewBag.ExternalMovements = externalMovements.Value ?? new();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInternal(
            Guid employeeId, Guid fromDepartmentId, Guid toDepartmentId, string reason,
            DateTime executionDate, Guid? newJobTitleId, IFormFile? attachmentFile)
        {
           

            var result = await _mediator.Send(new CreateInternalTransferCommand(
                employeeId, fromDepartmentId, toDepartmentId, reason, executionDate, newJobTitleId, attachmentFile));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تسجيل حركة النقل الداخلي بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveInternal(Guid transferId)
        {
            var result = await _mediator.Send(new ApproveInternalTransferCommand(transferId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم اعتماد حركة النقل." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExternal(
            Guid employeeId, ExternalMovementType type, MovementDirection direction, string otherEntityName,
            DateTime? startDate, DateTime? endDate, SalaryBearer? salaryBearer, IFormFile? attachmentFile)
        {
         

            var result = await _mediator.Send(new CreateExternalMovementCommand(
                employeeId, type, direction, otherEntityName, startDate, endDate, salaryBearer, attachmentFile));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تسجيل الحركة الخارجية بنجاح." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenewExternal(Guid movementId, DateTime newEndDate)
        {
            var result = await _mediator.Send(new RenewExternalMovementCommand(movementId, newEndDate));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم تجديد الندب." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndExternal(Guid movementId)
        {
            var result = await _mediator.Send(new EndExternalMovementCommand(movementId));

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "تم إنهاء الحركة." : result.Error.Name;

            return RedirectToAction(nameof(Index));
        }
    }
}