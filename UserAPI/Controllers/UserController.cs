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

        public override Task<IActionResult> AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> DeleteUser([FromRoute(Name = "username"), Required] string username)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> GetHistory([FromRoute(Name = "username"), Required] string username)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> GetUser([FromRoute(Name = "username"), Required] string username)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] PatchUserRequest patchUserRequest)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> SignUp([FromBody] PatchUserRequest patchUserRequest)
        {
            throw new NotImplementedException();
        }
    }
}
