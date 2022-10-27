using RideSaver.Server.Models;

namespace UserAPI.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(string username);
        void CreateUser(User user);
        void DeleteUser(string username);
        void UpdateUser(User user);

        void Save();
    }
}
