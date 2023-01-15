using Grpc.Core;
using IdentityService.Data;
using IdentityService.Interface;
using InternalAPI;
using Microsoft.EntityFrameworkCore;
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
            string headerToken = "" + _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if(headerToken is null) { throw new ArgumentNullException(nameof(headerToken)); }

            var cPrincipal = _tokenService.GetPrincipal(headerToken!);
            if(cPrincipal is null) { throw new ArgumentNullException(nameof(cPrincipal)); }

            var userID = cPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userID is null) { throw new ArgumentNullException(nameof(userID)); }

            var serviceID = request.ServiceId.ToString();
            if (serviceID is null) { throw new ArgumentNullException(nameof(serviceID)); }

            var accessToken = await _userContext.Authorizations!
                .Where(auth => auth.UserId.Equals(new Guid(userID)))
                .Where(auth => auth.ServiceId.Equals(new Guid(serviceID)))
                .FirstOrDefaultAsync(); // Retrieves the refresh-token matching the UID & service ID

            if (accessToken is null) { throw new ArgumentNullException(nameof(accessToken)); }

            return new GetUserAccessTokenResponse { AccessToken = accessToken.RefreshhToken };
        }
    }
}
