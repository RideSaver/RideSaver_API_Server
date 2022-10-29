using RideSaver.Server.Models;

namespace ServicesAPI.Repository
{
    public interface IServiceRepository
    {
        IEnumerable<RideService> GetAvailableServices();
        RideService GetService(Guid service_id);

    }
}
