using RideSaver.Server.Models;

namespace ServicesAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public RideService GetService(Guid service_id) // TBA
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RideService>> GetAvailableServices() // TBA:-> Will pull the service data from the DB.
        {
            throw new NotImplementedException();
        }
    }
}
