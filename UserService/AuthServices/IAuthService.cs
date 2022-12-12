using DataAccess.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI;
using UserService.Data;

namespace UserService.AuthServices
{
    public interface IAuthService
    {
        Task<IdentityResponse> GetIdentity(IdentityRequest request, ServerCallContext context);
        Task<Empty> PostIdentityToAuthService(UserModel model);
    }
}
