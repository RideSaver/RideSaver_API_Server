using RideSaver.Server.Models;

namespace EstimateAPI.Repository
{
    public interface IEstimateRepository
    {
        Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats, string token);
        Task<List<Estimate>> GetRideEstimatesRefreshAsync(List<Guid> ids, string token);
    }
}
