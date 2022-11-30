using RideSaver.Server.Models;

namespace RequestAPI.Repository
{
    public interface IRequestRepository
    {
        Task<Ride> CreateRideRequestAsync(Guid estimate_id);
        Task<Ride> GetRideRequestAsync(Guid ride_id);
        Task<PriceWithCurrency> CancelRideRequestAsync(Guid ride_id);
    }
}
