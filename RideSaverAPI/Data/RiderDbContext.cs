using Microsoft.EntityFrameworkCore;
using RideSaverAPI.Models;

namespace RideSaverAPI.Data
{
    public class RiderDbContext : DbContext // Inherits from DbContext which comes from EntityFramework Core -> Allows for direct communications with the DB.
    {
        public RiderDbContext(DbContextOptions<RiderDbContext> options) : base(options) { } // Constructor -> options allows for configuration of the connection string.
        public DbSet<Rider> Riders { get; set; } // Representation of the table in the database. 

    }
}
