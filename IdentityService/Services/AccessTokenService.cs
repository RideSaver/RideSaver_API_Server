using Grpc.Core;
using InternalAPI;

namespace IdentityService.Services
{
    public class AccessTokenService : Users.UsersBase
    {
        public async override Task<GetUserAccessTokenResponse> GetUserAccessToken(GetUserAccessTokenRequest request, ServerCallContext context)
        {
            var serviceID = "this-is-temporary-refresh-access-token"; // TODO -> Implement gRPC service-to-service authentication for claim principals.
            await Task.Delay(1000);
            return new GetUserAccessTokenResponse { AccessToken = serviceID};
        }
    }
}
