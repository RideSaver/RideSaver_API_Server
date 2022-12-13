using DataAccess.DataModels;
using System.Security.Claims;

namespace IdentityService.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserModel userInfo); // Generates JWT token based on the UserModel

        Task<bool> ValidateToken(string token);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipal(string token); // Retrieves the ClaimsPrinciple of the JWT token
    }
}
