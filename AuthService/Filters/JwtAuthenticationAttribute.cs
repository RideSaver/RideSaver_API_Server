using AuthService.Repository;
using AuthService.Services;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Filters;

namespace AuthService.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationRepository _authenticationRepository;
        public bool AllowMultiple => false;
        public string? Realm { get; set; }
        public JwtAuthenticationAttribute(ITokenService tokenService, IAuthenticationRepository authenticationRepository)
        {
            _tokenService = tokenService;
            _authenticationRepository = authenticationRepository;
        }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer") // Checks for missing headers
            {
                context.ErrorResult = new AuthenticationFailureResult("Authorization Header missing", request);
            }

            if (string.IsNullOrEmpty(authorization!.Parameter)) // Checks for the JWT token
            {
                context.ErrorResult = new AuthenticationFailureResult("JWT Token missing", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token); // Authenticates the token

            if (principal == null) context.ErrorResult = new AuthenticationFailureResult("JWT Token invalid", request);
            else context.Principal = principal;
        }

        private async Task<(bool isValid, string username)> ValidateToken(string token)
        {
            string? username = null;

            var simplePrinciple = _tokenService.GetPrincipal(token); // Retrieves the user principle from the token

            if (simplePrinciple?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            {
                return (false, null!); // Invalid identity/null flag
            }

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return (false, null!); // Invalid identity/null flag

            if (!await _authenticationRepository.ValidateUserAsync(username))
                return (false, null!); // Username exists within the database flag

            return (true, username);
        }

        protected async Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            var tokenInfo = await ValidateToken(token);
            if (tokenInfo.isValid)
            {
                var username = tokenInfo.username;
                var userInfo = await _authenticationRepository.GetUserAsync(username);

                var claims = new List<Claim> // Create a new Claim list based on the username from the DB.
                {
                    new Claim(ClaimTypes.Name, username!.ToString()),
                    new Claim(ClaimTypes.Email, userInfo.Email!.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return user;
            }

            return null;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string? parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter!);
        }
    }
}
