using AuthService.Filters;
using AuthService.Repository;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Attributes;
using RideSaver.Server.Models;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ILogger _logger;

        public AuthenticationController(ITokenService tokenService, IAuthenticationRepository authenticationRepository, ILogger logger)
        {
            _authenticationRepository = authenticationRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        [Consumes("application/json")]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin is null) return BadRequest();
            if (!await _authenticationRepository.AuthenticateUserAsync(userLogin)) return Unauthorized();

            var userInfo = await _authenticationRepository.GetUserAsync(userLogin.Username);
            _logger.LogInformation("[AuthenticationController] Login(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            return new OkObjectResult(_tokenService.GenerateToken(userInfo));
        }
    }
}
