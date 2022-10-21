using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace RideSaverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : EstimateApiController
    {
        public override IActionResult GetEstimates([FromQuery(Name = "startPoint"), Required] Location startPoint, [FromQuery(Name = "endPoint"), Required] Location endPoint, [FromQuery(Name = "services")] List<Guid> services, [FromQuery(Name = "seats")] int? seats)
        {
            throw new NotImplementedException();
        }

        public override IActionResult RefreshEstimates([FromQuery(Name = "ids"), Required] List<object> ids)
        {
            throw new NotImplementedException();
        }
    }
}
