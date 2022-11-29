using Grpc.Net.Client;
using InternalAPI;

namespace EstimateAPI.Repository
{
    public interface IEstimateRepository
    {
        Dictionary<string, Estimates.EstimatesClient> Clients { get; private set; };
    }
}
