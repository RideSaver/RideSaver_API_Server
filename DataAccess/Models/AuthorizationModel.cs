using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace DataAccess.Models
{
    [Table("authorizations")]
    [Index("UserId", "ServiceId", IsUnique = true, Name = "user_service")]
    public class AuthorizationModel
    {
        [Key]
        public Guid Id { get; set; }    

        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public string? RefreshToken { get; set; }

        [Required]
        [ForeignKey("ServiceId")]
        public virtual ProviderModel? ProviderModel { get; set; }  // navigation property

        [Required]
        [ForeignKey("UserId")]
        public virtual UserModel? UserModel { get; set; }  // navigation property
    }
}
