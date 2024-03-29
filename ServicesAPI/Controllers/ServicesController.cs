using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using ServicesAPI.Repository;

namespace ServicesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ServicesApiController
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServicesController> _logger;
        public ServicesController(IServiceRepository serviceRepository, ILogger<ServicesController> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }
        public override async Task<IActionResult> GetProviders()
        {
            _logger.LogInformation("[ServicesController::GetProviders] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            try
            {
                var rideServices = await _serviceRepository.GetAvailableProviders();
                return new OkObjectResult(rideServices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Internal Server Error");
            }
        }
        public override async Task<IActionResult> GetServices([FromQuery(Name = "location")] Location location)
        {
            _logger.LogInformation("[ServicesController::GetServices] Method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            try
            {
                var rideServices = await _serviceRepository.GetAvailableServices();
                return new OkObjectResult(rideServices);
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
