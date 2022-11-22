using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("providers")]
    public class ProviderModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? ClientId { get; set; }

        [Required]
        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<AuthorizationModel>? Authorizations { get; set; }
        public virtual ICollection<ServicesModel>? Services { get; set; }

    }
}
