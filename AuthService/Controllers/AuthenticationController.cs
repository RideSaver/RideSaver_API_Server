using AuthService.Data;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Attributes;
using UserLogin = AuthService.Models.UserLogin;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly AuthContext _credentialsContext;

        public AuthenticationController(ITokenService tokenService, AuthContext credentialsContext)
        {
            _tokenService = tokenService;
            _credentialsContext = credentialsContext;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        [Consumes("application/json")]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin) // returns HTTP 200 OK response with token (string)
        {
            var dbUser = await _credentialsContext
               .UserCredentials
               .SingleOrDefaultAsync(u => u.Username == userLogin.Username);

            if (dbUser == null)
            {
                return NotFound("User not found.");
            }

            var isValid = dbUser.Password == userLogin.Password;

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _tokenService.BuildToken(userLogin.Username);

            return Ok(token);
        }

        [HttpGet]
        [Route("/api/v1/auth/verifytoken")]
        [Consumes("application/json")]
        [ValidateModelState]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyToken()
        {
            var username = User
                .Claims
                .SingleOrDefault();

            if (username == null)
            {
                return Unauthorized();
            }

            var userExists = await _credentialsContext
                .UserCredentials
                .AnyAsync(u => u.Username == username.Value);

            if (!userExists)
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
