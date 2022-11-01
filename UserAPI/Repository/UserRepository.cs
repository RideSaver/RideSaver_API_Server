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
        public async Task<User> GetUserAsync(string username) => await _dbContext.Users.FindAsync(username);
        public Task SaveAsync() => _dbContext.SaveChangesAsync();

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.AddAsync(user);
            await SaveAsync();
        }
        public async Task DeleteUserAsync(string username)
        {
            var user = await _dbContext.Users.FindAsync(username);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await SaveAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await SaveAsync();
        }
        
    }
}
