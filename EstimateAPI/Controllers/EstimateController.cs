using EstimateAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace EstimateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstimateController : EstimateApiController
    {
        private readonly IEstimateRepository _estimateRepository;
        private readonly ILogger<EstimateController> _logger;
        public EstimateController(IEstimateRepository estimateRepository, ILogger<EstimateController> logger)
        {
            _estimateRepository = estimateRepository;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async override Task<IActionResult> GetEstimates([FromQuery(Name = "startPoint"), Required] Location startPoint, [FromQuery(Name = "endPoint"), Required] Location endPoint, [FromQuery(Name = "services")] List<Guid> services, [FromQuery(Name = "seats")] int? seats)
        {
            _logger.LogInformation("[EstimateController] GetEstimates(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesAsync(startPoint, endPoint, services, seats));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async override Task<IActionResult> RefreshEstimates([FromQuery(Name = "ids"), Required] List<Guid> ids)
        {
            _logger.LogInformation("[EstimateController] RefreshEstimates(); method invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesRefreshAsync(ids));
        }
    }
}
