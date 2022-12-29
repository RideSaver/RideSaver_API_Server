using EstimateAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace EstimateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstimateController : EstimateApiController
    {
        private readonly IEstimateRepository _estimateRepository;
        private readonly ILogger<EstimateController> _logger;
        public EstimateController(IEstimateRepository estimateRepository, ILogger<EstimateController> logger)
        {
            _estimateRepository = estimateRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        public async override Task<IActionResult> GetEstimates([FromQuery(Name = "startPoint"), Required] Location startPoint, [FromQuery(Name = "endPoint"), Required] Location endPoint, [FromQuery(Name = "services")] List<Guid> services, [FromQuery(Name = "seats")] int? seats)
        {
            var authorization = Request.Headers[HeaderNames.Authorization];
            string? token = null;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                token = headerValue.Parameter;
            }

            if (!string.IsNullOrEmpty(token)) { return BadRequest(); }

            _logger.LogInformation("[EstimateController] GetEstimates(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesAsync(startPoint, endPoint, services, seats, token));
        }

        [AllowAnonymous]
        public async override Task<IActionResult> RefreshEstimates([FromQuery(Name = "ids"), Required] List<Guid> ids)
        {
            var authorization = Request.Headers[HeaderNames.Authorization];
            string? token = null;

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                token = headerValue.Parameter;
            }

            if (!string.IsNullOrEmpty(token)) { return BadRequest(); }

            _logger.LogInformation("[EstimateController] RefreshEstimates(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesRefreshAsync(ids, token));
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
