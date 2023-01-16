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
            var userGUID = Guid.Parse(userID);

            var serviceID = request.ServiceId.ToString();
            if (serviceID is null) { throw new ArgumentNullException(nameof(serviceID)); }
            var serviceGUID = Guid.Parse(serviceID);

            var userAuthorization = await _userContext.Authorizations
               .Where(o => o.UserModelId == userGUID)
               .Where(f => f.ServiceId == serviceGUID)
               .FirstOrDefaultAsync();
            if(userAuthorization is null) { throw new ArgumentNullException(nameof(userAuthorization)); }

            return new GetUserAccessTokenResponse { AccessToken = userAuthorization.ServiceToken };
        }
    }
}
