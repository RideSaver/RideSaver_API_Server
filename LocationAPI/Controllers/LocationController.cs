using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace LocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : LocationApiController
    {
        public override Task<IActionResult> Autocomplete([FromHeader] Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            throw new NotImplementedException();
        }
    }
}
