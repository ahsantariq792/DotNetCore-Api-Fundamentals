using CRUD_Operation.Models.Auth;
using CRUD_Operation.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Signup(SignupRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.SignupAsync(request, cancellationToken);
            if (result == null)
                return BadRequest(new { message = "A user with this email already exists." });

            return Ok(result);
        }

        /// <summary>
        /// Login with email and password. Returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }
    }
}
