using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : LocationApiController
    {
        private readonly ILogger<LocationController> _logger;
        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }
        public override async Task<IActionResult> Autocomplete([FromQuery(Name = "location")] Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            _logger.LogInformation("[LocationController] Autocomplete(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());

            return new OkObjectResult(await Task.FromResult(new List<Location>()));
        }
    }
}
