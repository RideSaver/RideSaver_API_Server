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

        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            return new OkObjectResult(await _authenticationRepository.Authenticate(model));
        }

        [AllowAnonymous]
        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken(HttpAuthenticationContext context)
        {
            var token = context.Request.Headers.GetValues("token").FirstOrDefault();

            if (token == null) return new BadRequestResult();

            var isValid = await _authenticationRepository.ValidateToken(token);
            if (!isValid) return new UnauthorizedResult();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken model)
        {
            return new OkObjectResult(await _authenticationRepository.RefreshToken(model));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            return new OkObjectResult(await _authenticationRepository.RevokeToken(model));
        }
    }
}
