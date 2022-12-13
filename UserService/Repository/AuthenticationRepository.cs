using DataAccess.Models;
using IdentityService.Services;
using System.Runtime.InteropServices;

namespace IdentityService.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        public AuthenticationRepository(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            return (await _authService.Authenticate(model));
        }

        public async Task<AuthenticateResponse> RefreshToken(RefreshToken model)
        {
            return (await _authService.RefreshToken(model.Token));
        }

        public async Task<bool> RevokeToken(RevokeTokenRequest model)
        {
            return (await _authService.RevokeToken(model.Token));
        }

        public async Task<bool> ValidateToken(string token)
        {
            return (await _tokenService.ValidateToken(token));  
        }
    }
}
