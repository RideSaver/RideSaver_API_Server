using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideSaverAPI.Models;

namespace RideSaverAPI.Controllers
{
    [Route("api/[controller]")] // Specifies URL pattern for the API controller endpoints.
    [ApiController] // API controller attribute enables routing requirement & automatic http 400 responses.
    public class RideController : ControllerBase
    {
        public RideController() { }

        [HttpPost("RequestRide")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RequestRide(string riderCurrentLocation, string riderCurrentDestination)
        {
            var rideInstance = new Ride(riderCurrentLocation, riderCurrentDestination);
           // rideInstance.UberRideInstance = await UberAPI.FetchRideDetails(); // TO BE IMPLEMENTED IN THE UBERAPI
           // rideInstance.LyftRideInstance = await LyftAPI.FetchRideDetails(); // TO BE IMPLEMENTED IN THE LYFTAPI

            if (rideInstance.LyftRideInstance == null || rideInstance.UberRideInstance == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rideInstance);
            }      
        }

        [HttpPost("RequestLyftRide")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        
        public async Task<IActionResult> RequestLyftRide(Ride rideInstance)
        {
            // rideInstance.LyftRideInstance = await LyftAPI.RequestRide(); // TODO:: TO BE IMPLEMENTED AFTER LYFTAPI.
            return NotFound();
        }

        [HttpPost("RequestUberRide")]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> RequestUberRide(Ride rideInstance)
        {
            // rideInstance.UberRideInstance = await UberAPI.RequestRide(); // TODO:: TO BE IMPLEMENTED AFTER UBERAPI.
            return NotFound();
        }
    }
}
