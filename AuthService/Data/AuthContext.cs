using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
        public virtual DbSet<UserModel> UserCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProviderModel>()
            .Property(t => t.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("uniqueidentifier")
            .HasDefaultValueSql("(newsequentialid())");

            modelBuilder.Entity<List<Guid>>()
            .HasNoKey();

            modelBuilder.Entity<ProviderModel>()
           .HasMany(c => c.Authorizations)
           .WithOne(e => e.ProviderModel);
        }
    }
}
