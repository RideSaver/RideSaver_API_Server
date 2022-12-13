using InternalAPI;

namespace EstimateAPI.Repository
{
    public interface IClientRepository
    {
        Dictionary<string, Estimates.EstimatesClient> Clients { get; }
        Task RefreshClients();
    }
}
