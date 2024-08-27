using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var tokenResponse = _authService.Authenticate(loginRequest.Username, loginRequest.Password);
            if (tokenResponse == null)
            {
                return Unauthorized();
            }
            return Ok(tokenResponse);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenResponse = _authService.RefreshToken(request.Username, request.RefreshToken);
            if (tokenResponse == null)
            {
                return Unauthorized();
            }
            return Ok(tokenResponse);
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest request)
        {
            _authService.Logout(request.Username);
            return Ok();
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RefreshTokenRequest
        {
            public string Username { get; set; }
            public string RefreshToken { get; set; }
        }

        public class LogoutRequest
        {
            public string Username { get; set; }
        }
    }
}
