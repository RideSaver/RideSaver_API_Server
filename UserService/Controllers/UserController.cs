using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using UserService.Repository;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : UserApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public override async Task<IActionResult> SignUp([FromBody] PatchUserRequest patchUserRequest) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController] SignUp(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            using (var scope = new TransactionScope())
            {
                await _userRepository.CreateUserAsync(patchUserRequest);
                scope.Complete();
                return new OkResult(); // [STATUS: 204 NO CONTENT]
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> DeleteUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController] DeleteUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            await _userRepository.DeleteUserAsync(username);
            return new OkResult(); // [STATUS: 200 OK]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetUser([FromRoute(Name = "username"), Required] string username) // returns HTTP 200 OK response with "user" instance
        {
            _logger.LogInformation("[UserController] GetUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            var user = await _userRepository.GetUserAsync(username);
            if (user is not null) return new OkObjectResult(user); // [STATUS: 200 OK || user]
            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] PatchUserRequest patchUserRequest) // returns HTTP 200 OK response
        {
            _logger.LogInformation("[UserController] PatchUser(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            using (var scope = new TransactionScope())
            {
                await _userRepository.UpdateUserAsync(username, patchUserRequest);
                scope.Complete();
                return new OkResult(); // [STATUS: 200 OK]
            }
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
        public override Task<IActionResult> UpdateAvatar([FromRoute(Name = "username"), Required] string username, [FromBody] Stream body) => throw new NotImplementedException();
        public override Task<IActionResult> GetAvatar([FromRoute(Name = "username"), Required] string username) => throw new NotImplementedException();
        public override Task<IActionResult> Login([FromBody] UserLogin userLogin) => throw new NotImplementedException();

    }
}
