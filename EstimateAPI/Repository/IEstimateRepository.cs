using RideSaver.Server.Models;

namespace EstimateAPI.Repository
{
    public interface IEstimateRepository
    {
        Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        Task<List<Estimate>> GetLyftEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        Task<List<Estimate>> GetUberEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        List<Estimate> GetRideEstimatesRefresh(List<object> ids);
        Estimate GetLyftRideEstimateRefresh(Estimate estimate_id);
        Estimate GetUberRideEstimateRefresh(Estimate estimate_id);

    }
}
