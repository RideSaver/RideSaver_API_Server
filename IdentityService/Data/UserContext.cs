using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AuthorizationModel> Authorizations { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) {  }

    }
}
