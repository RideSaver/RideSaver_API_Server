using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;

namespace UserAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } // Context object for the users-DB.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasNoKey(); // TODO: Change OpenAPI specification & add a primary key for the user class.
            modelBuilder.Entity<List<Guid>>().HasNoKey();
        }
    }
}
