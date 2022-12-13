using DataAccess.Models;

namespace IdentityService.Services
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(string token);
        Task<bool> RevokeToken(string token);
    }
}
