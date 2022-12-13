using InternalAPI;

namespace RequestAPI.Repository
{
    public interface IClientRepository
    {
        Dictionary<string, Requests.RequestsClient> Clients { get; }
        Task RefreshClients();
    }
}
