using DataAccess.DataModels;
using RideSaver.Server.Models;
using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using IdentityService.Interface;
using IdentityService.Registry;
using System.Diagnostics.Contracts;

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
        public List<UserModel> GetUserModels() => _userContext.Users!.ToList();
        public async Task<UserModel> GetUserModelAsync(string username) => await _userContext.Users!.FindAsync(username);
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
                    Phonenumber = user.PhoneNumber,
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
                Phonenumber = userModel.PhoneNumber
            };

            return userInfo;
        }
        public async Task<bool> CreateUserAsync(PatchUserRequest userInfo)
        {
            if(userInfo is null) return false;
            if (await _userContext.Users!.AnyAsync(o => o.Username == userInfo.Username || o.Email == userInfo.Email)) return false;

            var salt = Security.Argon2.CreateSalt();
            var user = new UserModel()
            {
                Username = userInfo.Username,
                Name = userInfo.Name,
                Email = userInfo.Email,
                PhoneNumber = userInfo.Phonenumber,
                PasswordSalt = salt,
                PasswordHash = Security.Argon2.HashPassword(userInfo.Password, salt),
            };

            foreach(var serviceAuth in AuthorizationRegistry.ServiceAuth)
            {
                user.Authorizations.Add(serviceAuth);
            }

            await _userContext.Users!.AddAsync(user);

            _logger.LogInformation($"[userRepository::CreateUserAsync] {userInfo.Username} has been added to the Identity database.");

            await SaveAsync();
            return true; 
        }
        public async Task<bool> DeleteUserAsync(string username)
        {
            var userModel = await _userContext.Users!.FirstOrDefaultAsync(o => o.Username == username);
            if(userModel is null) return false;
            
            _userContext.Users!.Remove(userModel);
            await SaveAsync();
            return true;
        }
        public async Task<List<RideHistoryModel>> GetUserHistoryASync(string username)
        {
            var userModel = await _userContext.Users!.FirstOrDefaultAsync(o => o.Username == username);
            if (userModel is null) return null;
          
            return userModel.RideHistory?.ToList();
        }

        public async Task<bool> UpdateUserAsync(string username, PatchUserRequest userInfo)
        {
            var userModel = await _userContext.Users!.FirstOrDefaultAsync(o => o.Username == username);
            if (userModel is null) return false;

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
                userModel.PhoneNumber = userInfo.Phonenumber;
                _userContext.Update(userModel);
            }

            await SaveAsync();
            return true;
        }
    }
}
