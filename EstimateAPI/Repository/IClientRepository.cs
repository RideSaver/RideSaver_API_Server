using InternalAPI;

namespace EstimateAPI.Repository
{
    public interface IClientRepository
    {
        Estimates.EstimatesClient GetClientByName(string name);
        Task<List<Estimates.EstimatesClient>> GetClients();
    }
}
