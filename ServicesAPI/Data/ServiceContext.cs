using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace ServicesAPI.Data
{
    public class ServiceContext : DbContext
    {
        public DbSet<ProviderModel> Providers { get; set; }
        public DbSet<ServiceAreaModel> ServiceAreas { get; set; }
        public DbSet<ServicesModel> Services { get; set; }
        public DbSet<ServiceFeaturesModel> ServicesFeatures { get; set; }
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }

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
