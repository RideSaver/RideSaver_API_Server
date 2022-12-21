using InternalAPI;

namespace RequestAPI.Repository
{
    public interface IClientRepository
    {
        Requests.RequestsClient GetClientByName(string name, string token);
        Task<List<Requests.RequestsClient>> GetClients(string token);
    }
}
