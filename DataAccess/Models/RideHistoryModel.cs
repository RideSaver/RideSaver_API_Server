using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("ride_history")]
    public class RideHistoryModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]  
        public Guid ServiceId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string? Currency { get; set; }

        [Required]
        public string? Url { get; set; }

        [Timestamp]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel? UserModel { get; set; } // navigation property

        [ForeignKey("ServiceId")]
        public virtual ServicesModel? ServicesModel { get; set; } // navigation property

    }
}
