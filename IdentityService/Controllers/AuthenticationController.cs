using IdentityService.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Filters;
using IdentityService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly Logger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationRepository authenticationRepository, Logger<AuthenticationController> logger)
        {
            _authenticationRepository = authenticationRepository;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            _logger.LogInformation("[AuthenticationController] Authenticate(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            var auth = await _authenticationRepository.Authenticate(model);
            if (auth is null) return new BadRequestResult();
            return new OkObjectResult(auth);
        }

        [AllowAnonymous]
        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken(HttpAuthenticationContext context)
        {
            var token = context.Request.Headers.GetValues("token").FirstOrDefault();

            _logger.LogInformation("[AuthenticationController] ValidateToken(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            if (token == null) return new BadRequestResult();

            var isValid = await _authenticationRepository.ValidateToken(token);
            if (!isValid) return new UnauthorizedResult();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(HttpAuthenticationContext context)
        {
            var token = context.Request.Headers.GetValues("refresh_token").FirstOrDefault();

            _logger.LogInformation("[AuthenticationController] RefreshToken(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            if (token == null) return new BadRequestResult();

            return new OkObjectResult(await _authenticationRepository.RefreshToken(token));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(HttpAuthenticationContext context)
        {
            var token = context.Request.Headers.GetValues("revoke_token").FirstOrDefault();

            _logger.LogInformation("[AuthenticationController] RevokeToken(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            if (token == null) return new BadRequestResult();

            return new OkObjectResult(await _authenticationRepository.RevokeToken(token));
        }
    }
}
