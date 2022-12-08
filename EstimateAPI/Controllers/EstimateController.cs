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
            string authToken = string.Empty;

            Request.Headers.TryGetValue("Authorization", out StringValues authToken);
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesAsync(startPoint, endPoint, services, seats, authToken));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async override Task<IActionResult> RefreshEstimates([FromQuery(Name = "ids"), Required] List<Guid> ids)
        {
            string authToken = string.Empty;

            Request.Headers.TryGetValue("Authorization", out StringValues authToken);
            return new OkObjectResult(await _estimateRepository.GetRideEstimatesRefreshAsync(ids, authToken));
        }
    }
}
