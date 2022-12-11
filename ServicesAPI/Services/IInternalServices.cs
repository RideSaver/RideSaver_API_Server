using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI;

namespace ServicesAPI.Services
{
    public interface IInternalServices
    {
        Task GetServices(Empty request, IServerStreamWriter<ServiceModel> responseStream, ServerCallContext context);
        Task<Empty> RegisterService(RegisterServiceRequest request, ServerCallContext context);
        Task<ServiceModel> GetServiceByHash(GetServiceByHashRequest request, ServerCallContext context);
    }
}
