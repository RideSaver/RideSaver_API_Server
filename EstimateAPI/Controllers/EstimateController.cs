using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaver.Server.Controllers;
using RideSaver.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace EstimateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : EstimateApiController
    {
        public override IActionResult GetEstimates([FromQuery(Name = "startPoint"), Required] Location startPoint, [FromQuery(Name = "endPoint"), Required] Location endPoint, [FromQuery(Name = "services")] List<Guid> services, [FromQuery(Name = "seats")] int? seats)
        {
            List<Estimate> estimatesList = new List<Estimate>();

            //List<Estimate> uberList = UberAPI.getEstimatesAsync();
            //List<Estimate> lyftList = LyftAPI.getEstimatesAsync();

            //List<Estimate> estimatesList = uberList.Concat(lyftList).ToList();

            return new OkObjectResult(estimatesList);
        }

        public override IActionResult RefreshEstimates([FromQuery(Name = "ids"), Required] List<object> ids)
        {
           List<Estimate> estimateRefreshList = new List<Estimate>();
           
           /* - TO BE ADDED AFTER THE API-CLIENTS.
            
           Estimate estimate = new Estimate();
           foreach(var id in ids)
           {
                if (id == [UBER - UNIQUE - ID]
                {
                    estimate = uberAPI.getEstimateRefreshAsync(string estimate_id);
                    estimateRefreshList.Add(estimate);
                }
                else
                {
                    stimate = lyftAPI.getEstimateRefreshAsync(string estimate_id);
                    estimateRefreshList.Add(estimate);
                }
           }
            
           */

            return new OkObjectResult(estimateRefreshList);
        }
    }
}
