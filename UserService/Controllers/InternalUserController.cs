using Grpc.Core;
using InternalAPI;
using ServicesAPI.Repository;
using Google.Protobuf.WellKnownTypes;
using DataAccess.Models;

namespace UserService.Controllers
{
    // Summary: Handles all requests for estimates
    public class InternalUserController : Users.UsersBase
    {
        private readonly UserContext _userContext;
        public InternalServicesController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        Task<GetUserAccessTokenResponse> GetUserAccessToken(GetUserAccessTokenRequest request, grpc::ServerCallContext context)
        {
            var userId = User.Claims.FindFirst(JwtRegisteredClaimNames.NameId);
            return new GetUserAccessTokenResponse {
                AccessToken = await GetToken(userId, request.ServiceId);
            };
        }
    }
}
