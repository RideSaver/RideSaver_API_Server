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

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override IActionResult SignUp([FromBody] User user)
        {
            using (var scope = new TransactionScope())
            {
                _userRepository.CreateUser(user);
                scope.Complete();
                return CreatedAtAction(nameof(GetUser), new { username = user.Name }, user);
            }
        }
        public override IActionResult GetUser([FromRoute(Name = "username"), Required] string username)
        {
            var user = _userRepository.GetUser(username);
            return new OkObjectResult(user);
        }
        public override IActionResult PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] User user)
        {
            if (user != null)
            {
                using (var scope = new TransactionScope())
                {
                    _userRepository.UpdateUser(user);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        public override IActionResult DeleteUser([FromRoute(Name = "username"), Required] string username)
        {
            _userRepository.DeleteUser(username);
            return new OkResult();
        }

        public override IActionResult Login() // TO BE IMPLEMENTED
        {
            throw new NotImplementedException();
        }
        public override IActionResult AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code)
        {
            throw new NotImplementedException(); // TO BE IMPLEMENTED
        }
    }
}
