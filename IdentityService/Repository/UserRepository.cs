using DataAccess.Models;
using RideSaver.Server.Models;
using IdentityService.Data;

namespace IdentityService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task SaveAsync() => await _userContext.SaveChangesAsync();
        public List<UserModel> GetUserModels() => _userContext.Users.ToList();
        public async Task<UserModel> GetUserModelAsync(string username) => await _userContext.Users.FindAsync(username);
        public List<User> GetUsers()
        {
            var userList = new List<User>();
            var userModels = GetUserModels();
            foreach (var user in userModels)
            {
                var userInfo = new User()
                {
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                };

                userList.Add(userInfo);
            }

            return userList;
        }
        public async Task<User> GetUserAsync(string username)
        {
            var userModel = await GetUserModelAsync(username);
            if (userModel is not null)
            {
                var userInfo = new User()
                {
                    Email = userModel.Email,
                    Name = userModel.Name,
                    PhoneNumber = userModel.PhoneNumber
                };

                return userInfo;
            }
            else
            {
                return null;
            }

        }
        public async Task CreateUserAsync(PatchUserRequest userInfo)
        {
            if (userInfo is not null)
            {
                var salt = Security.Argon2.CreateSalt();
                var user = new UserModel()
                {
                    Username = userInfo.Username,
                    Name = userInfo.Name,
                    CreatedAt = DateTime.Now,
                    Email = userInfo.Email,
                    PhoneNumber = userInfo.PhoneNumber,
                    PasswordSalt = salt,
                    PasswordHash = Security.Argon2.HashPassword(userInfo.Password, salt)
                };

                await _userContext.AddAsync(user);
                await SaveAsync();
            }
        }

        public async Task DeleteUserAsync(string username)
        {
            var userModel = await _userContext.Users.FindAsync(username);
            if (userModel is not null)
            {
                _userContext.Users.Remove(userModel);
                await SaveAsync();
            }
        }
        public async Task<List<RideHistoryModel>> GetUserHistoryASync(string username)
        {
            var userModel = await _userContext.Users.FindAsync(username);
            if (userModel is not null)
            {
                return userModel.RideHistory?.ToList();
            }
            else
            {
                List<RideHistoryModel>? emptyRideHistory = null;
                return emptyRideHistory;
            }
        }

        public async Task UpdateUserAsync(string username, PatchUserRequest userInfo)
        {
            var userModel = await _userContext.Users.FindAsync(username);
            byte[] newSalt = Security.Argon2.CreateSalt();
            if (userModel is not null)
            {
                if (!Security.Argon2.VerifyHash(userInfo.Password, userModel.PasswordHash!, userModel.PasswordSalt!))
                {
                    userModel.PasswordHash = Security.Argon2.HashPassword(userInfo.Password, newSalt);
                    userModel.PasswordSalt = newSalt;
                }
                userModel.Username = userInfo.Username;
                userModel.Email = userInfo.Email;
                userModel.Name = userInfo.Name;
                userModel.PhoneNumber = userInfo.PhoneNumber;
                _userContext.Update(userModel);
            }

            await SaveAsync();
        }
    }
}
