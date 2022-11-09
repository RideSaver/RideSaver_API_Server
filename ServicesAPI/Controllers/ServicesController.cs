using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using ServicesAPI.Repository;

namespace ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ServicesApiController
    {
        private readonly IServiceRepository _serviceRepository;
        public ServicesController(IServiceRepository serviceRepository) =>_serviceRepository = serviceRepository;

        public override async Task<IActionResult> GetServices([FromHeader] Location location)
        {
            var rideServices = await _serviceRepository.GetAvailableServices();
            return new OkObjectResult(rideServices);
        }
    }
}
