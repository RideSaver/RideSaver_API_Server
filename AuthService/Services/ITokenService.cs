using DataAccess.Models;

namespace AuthService.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserModel userInfo);
    }
}
