using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;
using DataAccess.Models;
using System.Text;
using UserService.Data;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext) => _userContext = userContext;

        public async Task SaveAsync() => await _userContext.SaveChangesAsync();

        public List<User> GetUsers()
        {
            var userList = new List<User>();
            var userModels = GetUserModels();
            foreach(var user in userModels)
            {
                var userInfo = new User()
                {
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = "000-000-0000",
                    Avatar = user.Avatar,
                };

                userList.Add(userInfo);
            }

            return userList;
        }

        public async Task<User> GetUserAsync(string username)
        {
            var userModel = await GetUserModelAsync(username);
            if(userModel is not null)
            {
                var userInfo = new User()
                {
                    Email = userModel.Email,
                    Name = userModel.Name,
                    PhoneNumber = "000-000-0000",
                    Avatar = userModel.Avatar,
                };

                return userInfo;
            }
            else
            {
                return null;
            }

        }

        public async Task<UserModel> GetUserModelAsync(string username)
        {
            return await _userContext.Users.FindAsync(username);
        }

        public List<UserModel> GetUserModels()
        {
            return _userContext.Users.ToList();
        }
        public async Task CreateUserAsync(PatchUserRequest userInfo)
        {
            if(userInfo is not null)
            {
                var user = new UserModel()
                {
                    Username = userInfo.Username,
                    Password = userInfo.Password,
                    Name = userInfo.Name,
                    CreatedAt = DateTime.Now,
                    Avatar = userInfo.Avatar,
                    Email = userInfo.Email,
                };

                await _userContext.AddAsync(user);
                await SaveAsync();
            }
        }

        public async Task DeleteUserAsync(string username)
        {
            var userModel = await _userContext.Users.FindAsync(username);
            if(userModel is not null)
            {
                _userContext.Users.Remove(userModel);
                await SaveAsync();
            }
        }
        public async Task<List<RideHistoryModel>> GetUserHistoryASync(string username)
        {
            var userModel = await _userContext.Users.FindAsync(username);
            if(userModel is not null)
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
            if(userModel is not null)
            {
                userModel.Username = userInfo.Username;
                userModel.Password = userInfo.Password;
                userModel.Email = userInfo.Email;
                userModel.Name = userInfo.Name;
                userModel.Avatar = userInfo.Avatar;

                _userContext.Update(userModel);
            }

            await SaveAsync();
        }

        public async Task<string> GetToken(Guid userId, Guid serviceId)
        {
            return await (from a in _userContext.Authorizations
                where a.UserId == userId && a.ServiceId == request.service_id
                select a
            ).FirstOrDefaultAsync<AuthorizationModel>();
        }
    }
}
