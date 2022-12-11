using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationController : LocationApiController
    {
        private readonly ILogger _logger;
        public LocationController(ILogger logger)
        {
            _logger = logger;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> Autocomplete([FromQuery(Name = "location")] Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            _logger.LogInformation("[LocationController] Autocomplete(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await Task.FromResult(new List<Location>()));
        }
    }
}
