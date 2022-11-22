using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("service_areas")]
    public class ServiceAreaModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        public Geometry? Region { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServicesModel? ServicesModel { get; set; } // navigation property

    }
}
