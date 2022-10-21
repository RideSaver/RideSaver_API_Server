using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;

namespace RideSaverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : DefaultApiController
    {
        public override IActionResult GetServices([FromHeader] Location location)
        {
            throw new NotImplementedException();
        }
    }
}
