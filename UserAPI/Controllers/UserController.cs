using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using UserAPI.Repository;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : UserApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) => _userRepository = userRepository;

        public override async Task<IActionResult> SignUp([FromBody] User user)
        {
            using (var scope = new TransactionScope())
            {
                await _userRepository.CreateUserAsync(user);
                scope.Complete();
                return new OkResult(); // [STATUS: 204 NO CONTENT]
            }
        }
        public override async Task<IActionResult> GetUser([FromRoute(Name = "username"), Required] string username)
        {
            var user = await _userRepository.GetUserAsync(username);
            if(user is not null) new OkObjectResult(user); // [STATUS: 200 OK || user]
            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }
        public override async Task<IActionResult> PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] User user)
        {
            if (user is not null)
            {
                using (var scope = new TransactionScope())
                {
                    await _userRepository.UpdateUserAsync(user);
                    scope.Complete();
                    return new OkResult(); // [STATUS: 200 OK]
                }
            }
            return new NoContentResult(); // [STATUS: 204 NO CONTENT]
        }

        public override async Task<IActionResult> DeleteUser([FromRoute(Name = "username"), Required] string username)
        {
            await _userRepository.DeleteUserAsync(username);
            return new OkResult(); // [STATUS: 200 OK]
        }

        public override async Task<IActionResult> Login() // TO BE IMPLEMENTED
        {
            return new OkResult(); // [STATUS: 200 OK]
        }
        public override Task<IActionResult> AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code)
        {
            throw new NotImplementedException(); // TO BE IMPLEMENTED
        }
    }
}
