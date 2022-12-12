using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestAPI.Repository;
using RideSaver.Server.Controllers;
using System.ComponentModel.DataAnnotations;

namespace RequestAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RequestController : RequestApiController
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<RequestController> _logger;
        public RequestController(IRequestRepository requestRepository, ILogger<RequestController> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> CancelRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            _logger.LogInformation("[RequestController] CancelRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _requestRepository.CancelRideRequestAsync(new Guid(rideId)));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            _logger.LogInformation("[RequestController] GetRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _requestRepository.GetRideRequestAsync(new Guid(rideId)));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> RequestRide([FromRoute(Name = "estimateId"), Required] string estimateId)
        {
            _logger.LogInformation("[RequestController] RequestRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _requestRepository.CreateRideRequestAsync(new Guid(estimateId)));
        }
    }
}
