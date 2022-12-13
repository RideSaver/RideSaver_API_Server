using DataAccess.Models;


namespace IdentityService.Repository
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(RefreshToken model);
        Task<bool> RevokeToken(RevokeTokenRequest model);
        Task<bool> ValidateToken(string token);
        
    }
}
