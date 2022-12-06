using RideSaver.Server.Models;

namespace ServicesAPI.Repository
{
    public interface IServiceRepository
    {
        Task<IEnumerable<RideService>> GetAvailableServices();
        RideService GetService(Guid service_id);
    }
}
