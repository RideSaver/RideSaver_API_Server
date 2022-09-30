using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideSaverAPI.Data;
using RideSaverAPI.Models;

namespace RideSaverAPI.Controllers
{
    [Route("api/[controller]")] // Specifies URL pattern for API controller endpoints
    [ApiController] // API Controller attribute enables routing requirment & automatic http 400 responses
    public class RiderController : ControllerBase
    {
        private readonly RiderDbContext _context; // Represents the DB

        public RiderController(RiderDbContext context) => _context = context; // Contrusctor to initalize an instance of the DB.

        [HttpGet("GetByEmail")] // (api/Rider/GetByEmail) endpoint. -> retrieves the rider's account by email if it exists in the database.
        [ProducesResponseType(typeof(Rider), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetByEmail([FromQuery] string riderEmail)  // Returns an IActionResult based on the finding the rider's email or not -> fail/success(rider.object)
        {
            var riderList = await _context.Riders.ToListAsync(); // Returns a list of all users registered in the database.
            var rider = riderList.FirstOrDefault(o => o.RiderEmail == riderEmail); // Searches for a matching email
            return rider == null ? NotFound() : Ok(rider); 
        }

        [HttpPost("CreateUser")] // (api/Rider/CreateUser) endpoint -> creates a new rider instance into the database.
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRider(Rider rider)
        {
            await _context.Riders.AddAsync(rider); // Adds the new instance into the context object async.
            await _context.SaveChangesAsync(); // Saves the changes into the context.
            return CreatedAtAction(nameof(GetByEmail), new { id = rider.RiderID }, rider); //  Responds with the details 
        }

        [HttpPut("UpdateUser")]  // (api/Rider/UpdateUser) endpoint -> updates the Rider data field provided in the 2nd parameter
        public async Task<IActionResult> Update(string riderEmail, Rider rider)
        {
            if (riderEmail != rider.RiderEmail) return BadRequest(); // compares the email provided in the string with the instance email.
            _context.Entry(rider).State = EntityState.Modified; // updates the entry state
            await _context.SaveChangesAsync(); // saves the change async

            return NoContent(); // returns no content response
        }

        [HttpDelete("DeleteUser")] // (api/Rider/DeleteUser) endpoint -> deletes the ride data field provided by the parameter.
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string riderEmail)
        {
            var riderList = await _context.Riders.ToListAsync();
            var riderToDelete = riderList.FirstOrDefault(o => o.RiderEmail == riderEmail);

            if (riderToDelete == null) return NotFound();
              
            _context.Riders.Remove(riderToDelete); // Deletes the rider object from the _context.
            await _context.SaveChangesAsync(); // Saves the changes to the DB
    
            return NoContent(); 
        }

    }
}
