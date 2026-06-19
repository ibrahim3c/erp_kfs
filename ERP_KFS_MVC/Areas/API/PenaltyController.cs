using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyController : ControllerBase
    {
        private readonly IMediator mediator;

        public PenaltyController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPenalties()
        {
            // هنا يمكنك استدعاء الـ Mediator للحصول على قائمة الجزاءات
            // var result = await mediator.Send(new GetPenaltyListQuery());
            // if (result.IsFailure)
            //     return BadRequest(result.Error);
            // return Ok(result.Value);
            return Ok("قائمة الجزاءات"); // هذا مجرد مثال، قم بتعديل الكود حسب الحاجة
        }
    }
}
