using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    [Route("api/geography")]
    [ApiController]
    public class GeographyApiController : ControllerBase
    {
        private readonly IGeographyService _geographyService;

        public GeographyApiController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        [HttpGet("villages-by-city-center/{cityCenterId:guid}")]
        public async Task<IActionResult> GetVillagesByCityCenter(Guid cityCenterId)
        {
            var result = await _geographyService.GetVillagesByCityCenterIdAsync(cityCenterId);

            if (result.IsFailure)
                return BadRequest(result.Error.Name);

            var data = result.Value.Select(v => new
            {
                id = v.Id,
                name = v.Name
            });

            return Ok(data);
        }
    }
}
