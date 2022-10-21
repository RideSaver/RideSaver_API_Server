using RideSaver.Server.Models;
using RideSaverAPI.Models;

namespace RideSaverAPI.Interface
{
    public interface IRideServices
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string product_id);
        Task<Estimate> GetEstimateAsync();
        Task<PriceEstimate> GetPriceEstimateAsync();
        Task<TimeEstimate> GetTimeEstimateAsync();
        Task<PriceDetails> GetPriceDetailsAsyncs();
        Task<Request> GetRideRequestAsync(Product product_info);
        Task<Receipt> GetReceiptAsync(Request request_info);
        Task<Trip> GetTripDetailsAsync(Product productInfo);
        Task<User> GetUserAsync();
        Task<User> GetUserHistoryASync();
        Task<User> GetUserPaymentAsync();

    }
}
