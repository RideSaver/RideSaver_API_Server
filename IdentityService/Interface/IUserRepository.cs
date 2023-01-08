using DataAccess.DataModels;
using RideSaver.Server.Models;

namespace IdentityService.Interface
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(PatchUserRequest userInfo);
        Task<bool> DeleteUserAsync(string username);
        List<User> GetUsers();
        List<UserModel> GetUserModels();
        Task<User> GetUserAsync(string username);
        Task<UserModel> GetUserModelAsync(string username);
        Task<List<RideHistoryModel>> GetUserHistoryASync(string username);
        Task<bool> UpdateUserAsync(string username, PatchUserRequest userInfo);
        Task SaveAsync();
    }
}
