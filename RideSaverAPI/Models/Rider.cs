using System.ComponentModel.DataAnnotations;

namespace RideSaverAPI.Models
{
    public class Rider // Rider model class - represents each user registered on  the application.
    {
        [Required]
        public Guid RiderID { get; set; } // Unique ID for each rider instance.
        [Required]
        public string RiderEmail { get; set; } // Email of the rider.
        [Required]
        public string RiderUsername { get; set; } // Username of the rider. {account}
        [Required]
        public string RiderPassword { get; set; } // Password of the rider. {account}
        [Required]
        public string RiderName { get; set; } // Name of the rider.
        [Required]
        public string RiderPhoneNumber { get; set; } // Phone number of the rider.
        

        public Rider(string riderEmail, string riderUsername, string riderPassword, string riderName, string riderPhoneNumber) // Parametric constructor.
        {
            RiderID = Guid.NewGuid(); // Generates a unique ID for the new rider instance.
            RiderEmail = riderEmail;
            RiderUsername = riderUsername;
            RiderPassword = riderPassword;
            RiderName = riderName;
            RiderPhoneNumber = riderPhoneNumber;
        }
    }
}
