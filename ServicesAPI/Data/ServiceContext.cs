using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace ServicesAPI.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }
        public DbSet<ProviderModel> Providers { get; set; }
        public DbSet<ServicesModel> Services { get; set; }

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
