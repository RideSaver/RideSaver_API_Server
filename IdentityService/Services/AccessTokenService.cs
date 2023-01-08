using Grpc.Core;
using IdentityService.Data;
using IdentityService.Interface;
using InternalAPI;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Services
{
    public class AccessTokenService : Users.UsersBase
    {
        private readonly UserContext _userContext;
        private readonly ILogger<AccessTokenService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public AccessTokenService(UserContext userContext, ILogger<AccessTokenService> logger, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _userContext = userContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async override Task<GetUserAccessTokenResponse> GetUserAccessToken(GetUserAccessTokenRequest request, ServerCallContext context)
        {
            _logger.LogInformation("[IdentityService::AccessTokenService::GetUserAccessToken] Access Token request received...");

            string headerToken = "" + _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if(headerToken is null) { _logger.LogInformation("[IdentityService::AccessTokenService::GetUserAccessToken] Request Headers are null."); }

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Headers token: {headerToken}");

            var cPrincipal = _tokenService.GetPrincipal(headerToken!);

            var userID = cPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Retrieving Access token for UserID: {userID}...");

            var serviceID = request.ServiceId.ToString();

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Retrieving Access token for ServiceID: {serviceID}...");

            if (userID is null) return new GetUserAccessTokenResponse { AccessToken = String.Empty };

            _logger.LogDebug($"User ID: {userID}");
            _logger.LogDebug($"Service ID: {serviceID}");

            var accessToken = await _userContext.Authorizations!
                .Where(auth => auth.UserId.Equals(new Guid(userID)))
                .Where(auth => auth.ServiceId.Equals(new Guid(serviceID)))
                .FirstOrDefaultAsync(); // Retrieves the refresh-token matching the UID & service ID

            if(accessToken is null) return new GetUserAccessTokenResponse { AccessToken = String.Empty };

            _logger.LogInformation($"[IdentityService::AccessTokenService::GetUserAccessToken] Returning Access Token: {accessToken.RefreshhToken}");

            return new GetUserAccessTokenResponse { AccessToken = accessToken.RefreshhToken };
        }
    }
}
