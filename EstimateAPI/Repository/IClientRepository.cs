using InternalAPI;

namespace EstimateAPI.Repository
{
    public interface IClientRepository
    {
        Estimates.EstimatesClient GetClientByName(string name, string token);
        Task<List<Estimates.EstimatesClient>> GetClients(string token);
    }
}
