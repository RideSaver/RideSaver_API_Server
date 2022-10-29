using RideSaver.Server.Models;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        public List<Estimate> GetRideEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats)
        {
            List<Estimate> lyftEstimates = GetLyftEstimates(startPoint, endPoint, services, seats);
            List<Estimate> uberEstimates = GetUberEstimates(startPoint, endPoint, services, seats);
            return lyftEstimates.Concat(uberEstimates).ToList();
        }
        public List<Estimate> GetLyftEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            throw new NotImplementedException();
        }
        public List<Estimate> GetUberEstimates(Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            throw new NotImplementedException();
        }

        public List<Estimate> GetRideEstimatesRefresh(List<object> ids) // TBA
        {
            List<Estimate> rideEstimatesRefresh = new List<Estimate>();


           foreach(Estimate id in ids)
           {
                if (id is object) // TBA: Distinguish between uber & lyft ride guids.
                {
                    var estimateRefresh = GetUberRideEstimateRefresh(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                } else
                {
                    var estimateRefresh = GetLyftRideEstimateRefresh(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                }
           }

            return rideEstimatesRefresh;
        }

        public Estimate GetLyftRideEstimateRefresh(Estimate estimate_id) // TBA
        {
            throw new NotImplementedException();
        }

        public Estimate GetUberRideEstimateRefresh(Estimate estimate_id) // TBA
        {
            throw new NotImplementedException();
        }
    }
}
