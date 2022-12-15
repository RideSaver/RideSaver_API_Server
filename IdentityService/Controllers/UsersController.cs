using IdentityService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

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
            if (patchUserRequest is null)
            {
                _logger.LogInformation("[UserController] SignUp() -> patchUserRequest IS null!");
                return BadRequest();
            }

            _logger.LogInformation($"[UserController] SignUp() -> [PatchUserRequest]:: Email: {patchUserRequest.Email} Name: {patchUserRequest?.Name}" +
                $" Phone:{patchUserRequest.PhoneNumber} Username: {patchUserRequest.Username} Password: {patchUserRequest.Password}");

            await _userRepository.CreateUserAsync(patchUserRequest);
            _logger.LogInformation("[UserController] SignUp(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> DeleteUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response
        {
            await _userRepository.DeleteUserAsync(username);
            _logger.LogInformation("[UserController] DeleteUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkResult(); // [STATUS: 200 OK]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response with "user" instance
        {
            var user = await _userRepository.GetUserAsync(username);
            _logger.LogInformation("[UserController] GetUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            if (user is not null) return new OkObjectResult(user); // [STATUS: 200 OK || user]
            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] PatchUserRequest patchUserRequest) // returns HTTP 200 OK response
        {
            await _userRepository.UpdateUserAsync(username, patchUserRequest);
            _logger.LogInformation("[UserController] PatchUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkResult(); // [STATUS: 200 OK]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetHistory([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response with List<Ride>
        {
            _logger.LogInformation("[UserController] GetHistory(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _userRepository.GetUserHistoryASync(username));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override Task<IActionResult> AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code)
        {
            throw new NotImplementedException(); // returns HTTP 200 OK response
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override Task<IActionResult> UpdateAvatar([FromRoute(Name = "username"), Required] string username, [FromBody] Stream body) => throw new NotImplementedException();

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override Task<IActionResult> GetAvatar([FromRoute(Name = "username"), Required] string username) => throw new NotImplementedException();

        [HttpPost]
        [Route("/user-error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) return NotFound();

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [HttpPost]
        [Route("/user-error")]
        public IActionResult HandleError() => Problem();
    }
}
