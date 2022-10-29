using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using ServicesAPI.Repository;

namespace ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : DefaultApiController
    {
        private readonly IServiceRepository _serviceRepository;
        public ServicesController(IServiceRepository serviceRepository) =>_serviceRepository = serviceRepository;

        public override IActionResult GetServices([FromHeader] Location location)
        {
            var rideServices = _serviceRepository.GetAvailableServices();
            return new OkObjectResult(rideServices);
        }
    }
}
