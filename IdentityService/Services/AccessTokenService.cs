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
        private readonly ILogger<AccessTokenService> _logger;

        public AccessTokenService(UserContext userContext, ILogger<AccessTokenService> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        //[Authorize]
        public async override Task<GetUserAccessTokenResponse> GetUserAccessToken(GetUserAccessTokenRequest request, ServerCallContext context)
        {
            _logger.LogInformation("[IdentityService::AccessTokenService::GetUserAccessToken] Access Token request recieved...");

            var userID = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieves the UUID from the Claims

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Retrieving Access token for UserID: {userID}...");

            var serviceID = request.ServiceId.ToString();

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Retrieving Access token for ServiceID: {serviceID}...");

            if (userID is null) return new GetUserAccessTokenResponse { AccessToken = String.Empty };

            _logger.LogDebug($"User ID: {userID}");
            _logger.LogDebug($"Service ID: {serviceID}");

            var accessToken =  _userContext.Authorizations
                .Where(auth => auth.UserId.Equals(new Guid(userID)))
                .Where(auth => auth.ServiceId.Equals(new Guid(serviceID)))
                .FirstOrDefault(); // Retrieves the refresh-token matching the UID & service ID

            if(accessToken is null) return new GetUserAccessTokenResponse { AccessToken = String.Empty };

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Returning Access Token: {accessToken.RefreshhToken}");

            return new GetUserAccessTokenResponse { AccessToken = accessToken.RefreshhToken };
        }
    }
}
