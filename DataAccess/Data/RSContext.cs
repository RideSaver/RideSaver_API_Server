using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace DataAccess.Data
{
    public class RSContext : DbContext
    {
        public RSContext(DbContextOptions<RSContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AuthorizationModel> Authorizations { get; set; }
        public DbSet<RideHistoryModel> RideHistory { get; set; }
        public DbSet<ProviderModel> Providers { get; set; }
        public DbSet<ServiceAreaModel> ServiceAreas { get; set; }  
        public DbSet<ServicesModel> Services { get; set; }
        public DbSet<ServiceFeaturesModel> ServicesFeatures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("(newsequentialid())");

            modelBuilder.Entity<ProviderModel>()
                 .Property(t => t.Id)
                 .IsRequired()
                 .HasColumnName("Id")
                 .HasColumnType("uniqueidentifier")
                 .HasDefaultValueSql("(newsequentialid())");

            modelBuilder.Entity<List<Guid>>()
                .HasNoKey();
        }

        public virtual ProviderModel GetProviderForEstimate(string estimateId)
        {
            var estimateIdParameter = new ObjectParameter("EstimateId", estimateId);
            return ((IObjectContextAdapter)this)
                .ObjectContext
                .ExecuteFunction<ProviderModel>("GetProviderForEstimate", estimateIdParameter)
                .GetEnumerator()
                .Current;
        }
    }
}
