using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : AuthenticateApiController
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationRepository authenticationRepository, ILogger<AuthenticationController> logger)
        {
            _authenticationRepository = authenticationRepository;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Produces("application/json")]
        public override async Task<IActionResult> Authenticate([FromBody] UserLogin model)
        {
            var authModel = new AuthenticateRequest() { Username = model.Username, Password = model.Password };
            _logger.LogInformation("[AuthenticationController] Authenticate(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var auth = await _authenticationRepository.Authenticate(authModel);
            if (auth is null) return new BadRequestResult();
            return new OkObjectResult(auth);
        }

        [AllowAnonymous]
        [HttpGet("validate-token")]
        [Produces("application/json")]
        public async Task<IActionResult> ValidateToken([FromHeader(Name="token")] string? token)
        {
            if (token is null) return new UnauthorizedResult();
            var isValid = await _authenticationRepository.ValidateToken(token);
            if (!isValid) return new UnauthorizedResult();

            return new OkResult();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("refresh-token")]
        [Produces("application/json")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name="refresh_token")] string? token)
        {
            if (token is null) return new UnauthorizedResult();
            _logger.LogInformation("[AuthenticationController] RefreshToken(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _authenticationRepository.RefreshToken(token));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("revoke-token")]
        [Produces("application/json")]
        public async Task<IActionResult> RevokeTokenRefreshToken([FromHeader(Name ="revoke_token")] string? token)
        {
            if (token is null) return new BadRequestResult();
            _logger.LogInformation("[AuthenticationController] RevokeToken(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _authenticationRepository.RevokeToken(token));
        }

        [HttpPost]
        [Route("/auth-error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) return NotFound();

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [HttpPost]
        [Route("/auth-error")]
        public IActionResult HandleError() => Problem();
    }
}
