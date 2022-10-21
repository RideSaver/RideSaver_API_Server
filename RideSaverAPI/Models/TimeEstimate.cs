using RideSaver.Server.Models;

namespace RideSaverAPI.Models
{
    public class TimeEstimate
    {
        public string? picture { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public User? rideUser { get; set; }

    }
}
