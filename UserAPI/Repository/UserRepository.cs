using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;
using UserAPI.Data;

namespace UserAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _dbContext;

        public UserRepository(UserContext dbContext) => _dbContext = dbContext;
        public IEnumerable<User> GetUsers() => _dbContext.Users.ToList();
        public User GetUser(string username) => _dbContext.Users.Find(username);
        public void UpdateUser(User user) =>_dbContext.Entry(user).State = EntityState.Modified;
        public void Save() => _dbContext.SaveChanges();

        public void CreateUser(User user)
        {
            _dbContext.Add(user);
            Save();
        }
        public void DeleteUser(string username)
        {
            var user = _dbContext.Users.Find(username);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                Save();
            }
        }
    }
}
