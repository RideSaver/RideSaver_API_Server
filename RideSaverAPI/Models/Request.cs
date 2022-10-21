using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace RideSaverAPI.Models
{
    public class Request
    {
        [Required]
        public string? fare_id { get; set; }
        [Required]
        public Product? product { get; set; }
        [Required]
        public double start_latitude { get; set; }
        [Required]
        public double end_latitude { get; set; }
        [Required]
        public double start_longitude { get; set; }
        [Required]
        public double end_longitude { get; set; }

        [Required]
        public string? request_id { get; set; }

        
    }
}