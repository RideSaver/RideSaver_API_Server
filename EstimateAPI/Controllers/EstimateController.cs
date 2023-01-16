using EstimateAPI.Helpers;
using EstimateAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

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
            var token = Utility.GetAuthorizationToken(Request.Headers[HeaderNames.Authorization]);
            if (string.IsNullOrEmpty(token)) { return BadRequest("Invalid Authorization Token"); }

            if(startPoint is null || endPoint is null || services is null) { return BadRequest("Invalid Request Data");  }

            _logger.LogInformation("[EstimateController::GetEstimates] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            try
            {
                return new OkObjectResult(await _estimateRepository.GetRideEstimatesAsync(startPoint, endPoint, services, seats, token));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [AllowAnonymous]
        public async override Task<IActionResult> RefreshEstimates([FromQuery(Name = "ids"), Required] List<Guid> ids)
        {
            var token = Utility.GetAuthorizationToken(Request.Headers[HeaderNames.Authorization]);
            if (string.IsNullOrEmpty(token)) { return BadRequest("Invalid Authorization Token"); }

            if (ids is null) { return BadRequest("Invalid Request Data"); }

            _logger.LogInformation("[EstimateController::RefreshEstimates] RefreshEstimates(); Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            try
            {
                return new OkObjectResult(await _estimateRepository.GetRideEstimatesRefreshAsync(ids, token));
            }
            catch(Exception ex)
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
