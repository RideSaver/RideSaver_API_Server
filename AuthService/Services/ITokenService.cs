using DataAccess.Models;
using System.Security.Claims;

namespace AuthService.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserModel userInfo); // Generates JWT token based on the UserModel
        ClaimsPrincipal GetPrincipal(string token); // Retrieves the ClaimsPrinciple of the JWT token
    }
}
