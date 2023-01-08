using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RequestAPI.Repository;
using RideSaver.Server.Controllers;
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
        public override async Task<IActionResult> CancelRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            _logger.LogInformation("[RequestController] CancelRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var authorization = Request.Headers[HeaderNames.Authorization];
            string? token = null;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                _logger.LogDebug($"Access Token: {headerValue.Parameter}");
                token = headerValue.Parameter;
            }
            else
            {
                _logger.LogDebug($"Access token could not be read: {authorization}");
            }

            if (string.IsNullOrEmpty(token)) { return BadRequest(); }

            return new OkObjectResult(await _requestRepository.CancelRideRequestAsync(new Guid(rideId), token));
        }

        [AllowAnonymous]
        public override async Task<IActionResult> GetRide([FromRoute(Name = "rideId"), Required] string rideId)
        {
            _logger.LogInformation("[RequestController] GetRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var authorization = Request.Headers[HeaderNames.Authorization];
            string? token = null;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                _logger.LogDebug($"Access Token: {headerValue.Parameter}");
                token = headerValue.Parameter;
            }
            else
            {
                _logger.LogDebug($"Access token could not be read: {authorization}");
            }

            if (string.IsNullOrEmpty(token)) { return BadRequest(); }

            return new OkObjectResult(await _requestRepository.GetRideRequestAsync(new Guid(rideId), token));
        }

        [AllowAnonymous]
        public override async Task<IActionResult> RequestRide([FromRoute(Name = "estimateId"), Required] string estimateId)
        {
            _logger.LogInformation("[RequestController] RequestRide(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            var authorization = Request.Headers[HeaderNames.Authorization];
            string? token = null;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                _logger.LogDebug($"Access Token: {headerValue.Parameter}");
                token = headerValue.Parameter;
            }
            else
            {
                _logger.LogDebug($"Access token could not be read: {authorization}");
            }

            if (string.IsNullOrEmpty(token)) { return BadRequest(); }

            return new OkObjectResult(await _requestRepository.CreateRideRequestAsync(new Guid(estimateId), token));
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
