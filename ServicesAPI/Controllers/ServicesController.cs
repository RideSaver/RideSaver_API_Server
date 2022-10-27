using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;

namespace ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : DefaultApiController
    {
        public override IActionResult GetServices([FromHeader] Location location)
        {
            List<RideService> rideServices = new List<RideService>() {
                new RideService {
                    DisplayName = "lyftPool",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                 new RideService {
                    DisplayName = "uberPool",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                  new RideService {
                    DisplayName = "uber",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                   new RideService {
                    DisplayName = "lyft",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
            };

            return new OkObjectResult(rideServices);
        }
    }
}
