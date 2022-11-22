using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("service_features")]
    public class ServiceFeaturesModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        public Features Feature { get; set; }
        public enum Features
        {
            shared = 0,
            cash = 1,
            professional_driver = 2,
            self_driving = 3
        }

        [ForeignKey("ServiceId")]
        public virtual ServicesModel? ServicesModel { get; set; } // navigation property
    }
}
