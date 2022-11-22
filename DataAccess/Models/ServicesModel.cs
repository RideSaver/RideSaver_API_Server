using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("services")]
    public class ServicesModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? ClientId { get; set; }

        [Required]
        
        public Guid ProviderId { get; set; }

        [Required]
        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<ServiceFeaturesModel>? ServiceFeatures { get; set; }

        public virtual ICollection<ServiceAreaModel>? ServiceArea { get; set; }

        public virtual ICollection<RideHistoryModel>? RideHistory { get; set; }

        [ForeignKey("ProviderId")]
        public virtual ProviderModel? ProviderModel { get; set; } // navigation property
    }
}
