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
    public class LocationController : LocationApiController
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> Autocomplete([FromQuery(Name = "location")] Location location, [FromQuery(Name = "maxResponses"), Range(1, 50)] int? maxResponses)
        {
            return new OkObjectResult(await Task.FromResult(new List<Location>()));
        }
    }
}
