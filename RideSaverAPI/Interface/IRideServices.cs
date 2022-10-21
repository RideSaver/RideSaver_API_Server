using RideSaver.Server.Models;
using RideSaverAPI.Models;

namespace RideSaverAPI.Interface
{
    public interface IRideServices
    {
        Task<List<Product>> GetProductsAsync();
        Task<List<Estimate>> GetEstimatesAsync();
        Task<Request> GetRideRequestAsync(Estimate estimate_id);
        Task<Receipt> GetReceiptAsync(Estimate estimate_id);
        Task<Trip> GetTripDetailsAsync(Estimate estimate_id);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserHistoryASync(User userObject);
        Task<User> GetUserPaymentAsync(User userObject);

    }
}
