using RideSaver.Server.Models;

namespace RequestAPI.Repository
{
    public interface IRequestRepository
    {
        Task<Ride> CreateRideRequestAsync(Guid estimate_id, string token);
        Task<Ride> GetRideRequestAsync(Guid ride_id, string token);
        Task<PriceWithCurrency> CancelRideRequestAsync(Guid ride_id, string token);
        string GetAuthorizationToken(Microsoft.Extensions.Primitives.StringValues headers);
    }
}
