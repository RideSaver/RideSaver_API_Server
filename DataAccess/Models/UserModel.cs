using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("users")]
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public byte[]? Password { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public string? Avatar { get; set; }

        [Required]
        public string? Email { get; set; }

        public virtual ICollection<AuthorizationModel>? AuthorizedServices { get; set; }

        public virtual ICollection<RideHistoryModel>? RideHistory { get; set; }

    }
}
