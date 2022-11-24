using DataAccess.Models;
using RideSaver.Server.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task CreateUserAsync(PatchUserRequest userInfo);
        Task DeleteUserAsync(string username);
        List<User> GetUsers();
        List<UserModel> GetUserModels();
        Task<User>GetUserAsync(string username);
        Task<UserModel> GetUserModelAsync(string username);
        Task<List<RideHistoryModel>> GetUserHistoryASync(string username);
        Task UpdateUserAsync(string username, PatchUserRequest userInfo);
        Task SaveAsync();
    }
}
