using RideSaver.Server.Models;

namespace UserAPI.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        Task<bool> ValidateUser(LoginRequest loginInfo);
        Task<User> GetUserAsync(string username);
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(string username);
        Task UpdateUserAsync(User user);
        Task SaveAsync();
    }
}
