using ocuNotify.Interfaces;
using ocuNotify.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ocuNotify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("login-google")]
        public async Task<ActionResult<UserLoginResponse>> LoginWithGoogle([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var result = await _authenticationService.LoginWithGoogle(request.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Google login");
                return Unauthorized(new { message = "Invalid token or authentication failed." });
            }
        }
    }
}
