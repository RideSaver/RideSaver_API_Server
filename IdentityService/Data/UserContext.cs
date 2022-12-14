using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;

namespace IdentityService.Data
{
    public class UserContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbSet<UserModel> Users { get; set; }

        public UserContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, EF6Console.Migrations.Configuration>());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("IdentityDB"));
        }
    }
}
