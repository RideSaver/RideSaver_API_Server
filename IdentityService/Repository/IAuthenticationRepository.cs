using DataAccess.Models;


namespace IdentityService.Repository
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(string token);
        Task<bool> RevokeToken(string token);
        Task<bool> ValidateToken(string token);
    }
}
