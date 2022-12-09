using DataAccess.Models;
using RideSaver.Server.Models;

namespace AuthService.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> AuthenticateUserAsync(UserLogin userLogin);
        Task<bool> ValidateUserAsync(string userName);
        Task<UserModel> GetUserAsync(string userName);
    }
}
