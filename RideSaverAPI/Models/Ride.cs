using System.ComponentModel.DataAnnotations;

namespace RideSaverAPI.Models
{
    public class Ride
    {
        [Required]
        public string RiderCurrentLocation { get; set; } // The rider current location for pickup.
        [Required]
        public string RiderCurrentDestination { get; set; } // The rider current destination for dropoff.
        public LyftRide? LyftRideInstance { get; set; } // Instance for the lyft ride details. 
        public UberRide? UberRideInstance { get; set; } // Instance for the uber ride details

        public Ride(string riderCurrentLocation, string riderCurrentDestination) // Parameteric constructor
        {
            RiderCurrentLocation = riderCurrentLocation; 
            RiderCurrentDestination = riderCurrentDestination;
            LyftRideInstance = new LyftRide();
            UberRideInstance = new UberRide();
        }

    }
}
