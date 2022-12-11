using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using ServicesAPI.Repository;

namespace ServicesAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServicesController : ServicesApiController
    {
        private readonly IServiceRepository _serviceRepository;
        public ServicesController(IServiceRepository serviceRepository) =>_serviceRepository = serviceRepository;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetProviders()
        {
            var rideServices = await _serviceRepository.GetAvailableProviders();
            return new OkObjectResult(rideServices);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetServices([FromQuery(Name = "location")] Location location)
        {
            var rideServices = await _serviceRepository.GetAvailableProviders();
            return new OkObjectResult(rideServices);
        }
    }
}
