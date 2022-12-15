using DataAccess.DataModels;
using RideSaver.Server.Models;
using IdentityService.Data;

namespace IdentityService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(UserContext userContext, ILogger<UserRepository> logger)
        {
            _userContext = userContext;
            _logger = logger;
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
                    PhoneNumber = user.PhoneNumber,
                };

                userList.Add(userInfo);
            }

            return userList;
        }
        public async Task<User> GetUserAsync(string username)
        {
            var userModel = await GetUserModelAsync(username);
            if (userModel is null) return null;

            var userInfo = new User()
            {
                Email = userModel.Email,
                Name = userModel.Name,
                PhoneNumber = userModel.PhoneNumber
            };

            return userInfo;
        }
        public async Task CreateUserAsync(PatchUserRequest userInfo)
        {
            if(userInfo is null) return;

            _logger.LogInformation("[userRepository] CreateUserAsync() -> Before creating the salt!");

            var salt = Security.Argon2.CreateSalt();

            _logger.LogInformation("[userRepository] CreateUserAsync() -> Before creating the userModel instance!");

            var user = new UserModel()
            {
                Id = new Guid(),
                Username = userInfo.Username.ToString(),
                Name = userInfo.Name.ToString(),
                Email = userInfo.Email.ToString(),
                PhoneNumber = userInfo.PhoneNumber.ToString(),
                PasswordSalt = salt,
                PasswordHash = Security.Argon2.HashPassword(userInfo.Password, salt)
                    
            };

            _logger.LogInformation($"[userRepository] UserModel() -> Id: {user.Id}");
            _logger.LogInformation($"[userRepository] UserModel() -> Name:{user.Name}");
            _logger.LogInformation($"[userRepository] UserModel() -> Email: {user.Email}");
            _logger.LogInformation($"[userRepository] UserModel() -> PhoneNumber: {user.PhoneNumber}");
            _logger.LogInformation($"[userRepository] UserModel() -> PasswordSalt: {user.PasswordSalt}");
            _logger.LogInformation($"[userRepository] UserModel() -> PasswordHash: {user.PasswordHash}");
            _logger.LogInformation($"[userRepository] UserModel() -> Username:  {user.Username}");

            await _userContext.AddAsync(user);

            _logger.LogInformation("[userRepository] CreateUserAsync() -> Just added to the _userContext ASYNC!");

            await SaveAsync();

            _logger.LogInformation("[userRepository] CreateUserAsync() -> Just SAVED to the _userContext ASYNC!");
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
