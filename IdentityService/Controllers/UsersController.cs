using IdentityService.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : UserApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        public override async Task<IActionResult> SignUp([FromBody] PatchUserRequest patchUserRequest) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController::SignUp] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            if (patchUserRequest is null) return BadRequest("Invalid user information!");

            var isValid = await _userRepository.CreateUserAsync(patchUserRequest);

            if (!isValid) return BadRequest("Account already exists!");

            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> DeleteUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController::DeleteUser] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var isValid = await _userRepository.DeleteUserAsync(username);
            if (!isValid) return BadRequest("Username does not exist!");

            return new OkResult(); // [STATUS: 200 OK]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response with "user" instance
        {
            _logger.LogInformation("[UserController::GetUser] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var user = await _userRepository.GetUserAsync(username);
            if (user is not null) return new OkObjectResult(user); // [STATUS: 200 OK || user]
            return BadRequest("User does not exist!");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] PatchUserRequest patchUserRequest) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController::PatchUser] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var isValid = await _userRepository.UpdateUserAsync(username, patchUserRequest);
            if (!isValid) return BadRequest("Username does not exist!");

            return new OkResult(); // [STATUS: 200 OK]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetHistory([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response with List<Ride>
        {
            _logger.LogInformation("[UserController::GetHistory] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var userHistory = await _userRepository.GetUserHistoryASync(username);

            if (userHistory is null) return new BadRequestObjectResult(userHistory);

            return new OkObjectResult(userHistory);
        }

        public override Task<IActionResult> AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code) => throw new NotImplementedException();
        public override Task<IActionResult> UpdateAvatar([FromRoute(Name = "username"), Required] string username, [FromBody] Stream body) => throw new NotImplementedException();
        public override Task<IActionResult> GetAvatar([FromRoute(Name = "username"), Required] string username) => throw new NotImplementedException();

        [HttpPost]
        [Route("/user-error-development")]
        protected IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) return NotFound();

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [HttpPost]
        [Route("/user-error")]
        protected IActionResult HandleError() => Problem();
    }
}
