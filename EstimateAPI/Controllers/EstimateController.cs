using EstimateAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace EstimateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : EstimateApiController
    {
        private readonly IEstimateRepository _estimateRepository;
        public EstimateController(IEstimateRepository estimateRepository) => _estimateRepository = estimateRepository;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async override Task<IActionResult> GetEstimates([FromQuery(Name = "startPoint"), Required] Location startPoint, [FromQuery(Name = "endPoint"), Required] Location endPoint, [FromQuery(Name = "services")] List<Guid> services, [FromQuery(Name = "seats")] int? seats)
        {
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesAsync(startPoint, endPoint, services, seats));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async override Task<IActionResult> RefreshEstimates([FromQuery(Name = "ids"), Required] List<object> ids)
        {
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesRefreshAsync(ids));
        }
    }
}
