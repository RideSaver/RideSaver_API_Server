using RideSaver.Server.Models;
using ServiceProvider = RideSaver.Server.Models.ServiceProvider;

namespace ServicesAPI.Repository
{
    public interface IServiceRepository
    {
        Task<List<RideService>> GetAvailableServices();
        Task<List<ServiceProvider>> GetAvailableProviders();
    }
}
