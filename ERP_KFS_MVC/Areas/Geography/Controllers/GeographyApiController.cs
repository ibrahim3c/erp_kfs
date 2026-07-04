using Geography.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Geography.Controllers
{
    /// <summary>
    ///     API for geographical data lookups (governorates, cities, villages).
    /// </summary>
    [Route("api/geography")]
    [ApiController]
    public class GeographyApiController : ControllerBase
    {
        private readonly IGeographyService _geographyService;

        public GeographyApiController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        /// <summary>
        ///     Retrieves all villages belonging to a specific city center.
        /// </summary>
        /// <param name="cityCenterId">The city center's unique identifier.</param>
        /// <returns>List of villages (id + name) under the specified city center.</returns>
        /// <response code="200">Returns list of villages.</response>
        /// <response code="400">Invalid city center ID or lookup failed.</response>
        [HttpGet("villages-by-city-center/{cityCenterId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
