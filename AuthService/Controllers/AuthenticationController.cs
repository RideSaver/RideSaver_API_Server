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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(ITokenService tokenService, IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
            _tokenService = tokenService;
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

            return new OkObjectResult(_tokenService.GenerateToken(userInfo));
        }
    }
}
