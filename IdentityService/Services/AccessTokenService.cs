using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IdentityService.Data;
using InternalAPI;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityService.Services
{
    public class AccessTokenService : Users.UsersBase
    {
        private readonly UserContext _userContext;

        public AccessTokenService(UserContext userContext)
        {
            _userContext = userContext;
        }

        [Authorize]
        public async override Task<GetUserAccessTokenResponse> GetUserAccessToken(GetUserAccessTokenRequest request, ServerCallContext context)
        {
            var userID = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieves the UUID from the Claims

            var serviceID = request.ServiceId;

            if(userID is null) return new GetUserAccessTokenResponse { AccessToken = null };

            var accessToken = await _userContext.Authorizations
                .Where(auth => auth.UserID.Equals(userId))
                .Where(auth => auth.ServiceID.Equals(serviceID))
                .FirstOrDefault(); // Retrieves the refresh-token matching the UID & service ID

            if(accessToken is null) return new GetUserAccessTokenResponse { AccessToken = null };

            return new GetUserAccessTokenResponse { AccessToken = serviceID};
        }
    }
}
