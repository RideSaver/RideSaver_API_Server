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

        public override async Task<IActionResult> RidesRequestEstimateIdPost([FromRoute(Name = "estimateId")] string estimateId)
        {
            return new OkObjectResult(await _requestRepository.GetRideRequestIDAsync(new Guid(estimateId)));
        }

        public override async Task<IActionResult> RidesRideIdDelete([FromRoute(Name = "rideId")] string rideId)
        {
            return new OkObjectResult(await _requestRepository.DeleteRideRequestAsync(new Guid(rideId)));
        }

        public override async Task<IActionResult> RidesRideIdGet([FromRoute(Name = "rideId")] string rideId)
        {
            return new OkObjectResult(await _requestRepository.GetRideRequestIDAsync(new Guid(rideId)));
        }
    }
}
