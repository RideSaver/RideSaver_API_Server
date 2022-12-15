using Geocoding;
using Geocoding.Microsoft;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using System.ComponentModel.DataAnnotations;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : LocationApiController
    {
        private readonly ILogger<LocationController> _logger;

        private BingMapsGeocoder geoCoder; // Bings Map GeoCoder
        private string API_KEY = "CVGYROGSUOCdFA9hBI7Zf0OhV2p30kc9MKjq0WmxGskghPuZd3RA_Dhh5Mwu4"; // Bings Map API Key

        public LocationController(ILogger<LocationController> logger)
        {
            geoCoder = new BingMapsGeocoder(API_KEY);
            _logger = logger;
        }

        public override Task<IActionResult> Autocomplete([FromQuery(Name = "location")] RideSaver.Server.Models.Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            throw new NotImplementedException();
        }

        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) return NotFound();

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [Route("/error")]
        public IActionResult HandleError() => Problem();
    }
}
