using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAPI.Repository;
using RideSaver.Server.Controllers;

namespace RequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : RequestApiController
    {
        private readonly IRequestRepository _requestRepository;
        public RequestController(IRequestRepository requestRepository) => _requestRepository = requestRepository;

        public override Task<IActionResult> RidesRequestEstimateIdPost([FromRoute(Name = "estimateId")] string estimateId)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> RidesRideIdDelete([FromRoute(Name = "rideId")] string rideId)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> RidesRideIdGet([FromRoute(Name = "rideId")] string rideId)
        {
            throw new NotImplementedException();
        }
    }
}
