
using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;

namespace UserAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } // Context object for the users-DB.
        protected override void OnModelCreating(ModelBuilder modelBuilder) { /* TBA */ }
        
    }
}
