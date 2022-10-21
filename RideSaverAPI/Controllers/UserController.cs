using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace RideSaverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : UserApiController
    {
        public override IActionResult AutorizeServiceEndpoint([FromRoute(Name = "serviceId"), Required] Guid serviceId, [FromRoute(Name = "userId"), Required] Guid userId, [FromQuery(Name = "code"), Required] string code)
        {
            throw new NotImplementedException();
        }

        public override IActionResult DeleteUser([FromRoute(Name = "username"), Required] string username)
        {
            throw new NotImplementedException();
        }

        public override IActionResult GetUser([FromRoute(Name = "username"), Required] string username)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Login()
        {
            throw new NotImplementedException();
        }

        public override IActionResult PatchUser([FromRoute(Name = "username"), Required] string username, [FromBody] User user)
        {
            throw new NotImplementedException();
        }

        public override IActionResult SignUp([FromBody] User user)
        {
            throw new NotImplementedException();
        }
    }
}
