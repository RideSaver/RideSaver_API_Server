using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RequestAPI.Helpers;
using RequestAPI.Repository;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace RequestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestController : RequestApiController
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<RequestController> _logger;
        public RequestController(IRequestRepository requestRepository, ILogger<RequestController> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        public override async Task<IActionResult> GetRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            var token = Utility.GetAuthorizationToken(Request.Headers[HeaderNames.Authorization]);
            if (string.IsNullOrEmpty(token)) { return BadRequest("Invalid Authorization Token!"); }

            if (rideId is null) { return BadRequest("Invalid Ride ID!"); }

            _logger.LogInformation("[RequestController::GetRide] Method invoked at {DT}.", DateTime.UtcNow.ToLongTimeString());

            try
            {
                var requestUID = Guid.Parse(rideId.ToString());
                return new OkObjectResult(await _requestRepository.GetRideRequestAsync(requestUID, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [AllowAnonymous]
        public override async Task<IActionResult> CancelRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            var token = Utility.GetAuthorizationToken(Request.Headers[HeaderNames.Authorization]);
            if (string.IsNullOrEmpty(token)) { return BadRequest(); }

            if (rideId is null) { return BadRequest("Invalid Ride ID!"); }

            _logger.LogInformation("[RequestController::CancelRide] Method invoked at {DT}.", DateTime.UtcNow.ToLongTimeString());

            try
            {
                var requestUID = Guid.Parse(rideId.ToString());
                return new OkObjectResult(await _requestRepository.CancelRideRequestAsync(requestUID, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [AllowAnonymous]
        public override async Task<IActionResult> RequestRide([FromRoute(Name = "estimateId"), Required] string estimateId)
        {
            var token = Utility.GetAuthorizationToken(Request.Headers[HeaderNames.Authorization]);
            if (string.IsNullOrEmpty(token)) { return BadRequest(); }

            if (estimateId is null) { return BadRequest("Invalid Estimate ID!"); }

            _logger.LogInformation("[RequestController::RequestRide] Method invoked at {DT}.", DateTime.UtcNow.ToLongTimeString());

            try
            {
                var estimateUID = Guid.Parse(estimateId.ToString());
                return new OkObjectResult(await _requestRepository.CreateRideRequestAsync(estimateUID, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [Route("/error-development")]
        protected IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) return NotFound();

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [Route("/error")]
        protected IActionResult HandleError() => Problem();
    }
}
