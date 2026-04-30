using AuthService.Contracts.Auth;
using AuthService.Application.UseCases.Login;
using AuthService.Application.UseCases.Register;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterHandler _registerHandler;
        private readonly LoginHandler _loginHandler;

        public AuthController(
            RegisterHandler registerHandler,
            LoginHandler loginHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
        }

        // -------------------- LOGIN --------------------

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var command = new LoginCommand(
                email,
                request.Password);

            var token = await _loginHandler.Handle(command);

            return Ok(new LoginResponse
            {
                AccessToken = token
            });
        }

        // -------------------- REGISTER --------------------

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisterResponse>> Register(
            [FromBody] RegisterRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var command = new RegisterCommand(
                email,
                request.Password);

            var userId = await _registerHandler.Handle(command);

            return Ok(new RegisterResponse
            {
                UserId = userId,
                Email = email
            });
        }
    }
}
