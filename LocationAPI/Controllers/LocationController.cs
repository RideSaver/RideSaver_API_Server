using Geocoding;
using Geocoding.Microsoft;
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

        private BingMapsGeocoder geoCoder;
        private string API_KEY = "CVGYROGSUOCdFA9hBI7Zf0OhV2p30kc9MKjq0WmxGskghPuZd3RA_Dhh5Mwu4";

        public LocationController(ILogger<LocationController> logger)
        {
            geoCoder = new BingMapsGeocoder(API_KEY);
            _logger = logger;
        }

        public override Task<IActionResult> Autocomplete([FromQuery(Name = "location")] RideSaver.Server.Models.Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            throw new NotImplementedException();
        }
    }
}
