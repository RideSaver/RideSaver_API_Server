using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using ServicesAPI.Repository;

namespace ServicesAPI.Controllers
{
    [ApiController]
    public class ServicesController : ServicesApiController
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServicesController> _logger;
        public ServicesController(IServiceRepository serviceRepository, ILogger<ServicesController> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetProviders()
        {
            _logger.LogInformation("[ServicesController] GetProviders(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            var rideServices = await _serviceRepository.GetAvailableProviders();
            return new OkObjectResult(rideServices);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetServices([FromQuery(Name = "location")] Location location)
        {
            _logger.LogInformation("[ServicesController] GetServices(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            var rideServices = await _serviceRepository.GetAvailableProviders();
            return new OkObjectResult(rideServices);
        }
    }
}
