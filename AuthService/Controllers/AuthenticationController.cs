/**
 * \author John Hanna
 * \brief Authentication HTTP(S) Controller Class
 */
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
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly AuthContext _credentialsContext;

        public AuthenticationController(ITokenService tokenService, AuthContext credentialsContext, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _credentialsContext = credentialsContext;
            _configuration = configuration;
        }

        /// @startuml "State Diagram"
        /// start
        /// :Username<
        /// :Password<
        /// :Get User Information form Database with username;
        /// if (User Exists?) then (no)
        ///     :return NotFound("ERROR: user not found")>
        ///     stop
        /// endif
        /// -> yes;
        /// :Check Password matches stored password;
        /// if (Password is same as stored password?) then (no)
        ///     :return BadRequest("ERROR: Failed to authenticate user")>
        ///     stop
        /// endif
        /// -> yes;
        /// :return JWT token with user information.>
        /// stop
        /// @enduml
        ///
        /// @startuml "Sequence Diagram"
        /// participant Caller
        /// participant "Authorization Service" as AS
        /// database AuthDB
        /// Caller -> AS ++: Login
        /// AS -> AuthDB ++: Get User by Username
        /// AuthDB -> AuthDB: Search Users
        /// return User Information
        /// alt Correct Username / Password
        ///     AS -[dashed]-> Caller: JWT Token
        /// else Incorrect Username
        ///     AS -[dashed]-> Caller: NotFound("ERROR: user not found")
        /// else Incorrect Password
        ///     return BadRequest("ERROR: Failed to authenticate user")
        /// end
        /// @enduml
        [HttpPost]
        [Route("/api/v1/auth/login")]
        [Consumes("application/json")]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var userInfo = await _credentialsContext.UserCredentials.SingleOrDefaultAsync(u => u.Username == userLogin.Username);

            if (userInfo is null)
            {
                return NotFound("ERROR: user not found");
            }

            var isValid = userInfo.Password == userLogin.Password; // TODO: encryption/hashing/salting for user passwords.

            if (!isValid)
            {
                return BadRequest("ERROR: Failed to authenticate user");
            }

            return Ok(_tokenService.GenerateToken(userInfo));
        }

        [HttpGet]
        [Route("/api/v1/auth/verifytoken")]
        [Consumes("application/json")]
        [ValidateModelState]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyToken()
        {
            var username = User.Claims.SingleOrDefault();

            if (username is null)
            {
                return Unauthorized();
            }

            var userExists = await _credentialsContext.UserCredentials.AnyAsync(u => u.Username == username.Value);

            if (!userExists)
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
