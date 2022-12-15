using InternalAPI;

namespace EstimateAPI.Repository
{
    public interface IClientRepository
    {
        Estimates.EstimatesClient GetClientByName(string name);
        Task<Estimates.EstimatesClient[]> GetClients();
    }
}
