using RideSaver.Server.Models;

namespace EstimateAPI.Repository
{
    public interface IEstimateRepository
    {
        List<Estimate> GetRideEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        List<Estimate> GetLyftEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        List<Estimate> GetUberEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats);
        List<Estimate> GetRideEstimatesRefresh(List<object> ids);
        Estimate GetLyftRideEstimateRefresh(Estimate estimate_id);
        Estimate GetUberRideEstimateRefresh(Estimate estimate_id);

    }
}
