using RideSaver.Server.Models;

namespace RequestAPI.Repository
{
    public interface IRequestRepository
    {
        Task<Ride> PostRideRequestAsync(Guid estimate_id);
        Task<Ride> PostLyftRideRequestAsync(Guid estimate_id);
        Task<Ride> PostUberRideRequestAsync(Guid estimate_id);
        Task<Ride> GetRideRequestIDAsync(Guid ride_id);
        Task<Ride> GetLyftRideRequestIDAsync(Guid ride_id);
        Task<Ride> GetUberRideRequestIDAsync(Guid ride_id);
        Task<PriceWithCurrency> DeleteRideRequestAsync(Guid ride_id);
        Task<PriceWithCurrency> DeleteLyfttRideRequestAsync(Guid ride_id);
        Task<PriceWithCurrency> DeleteUberRideRequestAsync(Guid ride_id);
    }
}
