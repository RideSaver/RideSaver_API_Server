using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }
        public DbSet<ProviderModel>? Providers { get; set; }
        public DbSet<ServicesModel>? Services { get; set; }

    }
}
