using Identity.Application.Dtos;
using Identity.Application.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService authService;

        public AccountController(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        ///     Authenticates a user and returns a JWT token for subsequent API requests.
        /// </summary>
        /// <param name="loginDto">Login credentials (email, password, rememberMe flag).</param>
        /// <returns>Authenticated user info with JWT access token and refresh token.</returns>
        /// <response code="200">Login successful — returns user info and JWT token.</response>
        /// <response code="400">Login failed — invalid credentials or validation error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await authService.LoginJwtAsync(loginDto);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
