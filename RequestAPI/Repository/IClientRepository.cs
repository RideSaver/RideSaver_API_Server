using InternalAPI;

namespace RequestAPI.Repository
{
    public interface IClientRepository
    {
        Requests.RequestsClient GetClientByName(string name);
        Task<List<Requests.RequestsClient>> GetClients();
    }
}
