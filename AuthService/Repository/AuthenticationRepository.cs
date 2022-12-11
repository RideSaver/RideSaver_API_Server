using AuthService.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using UserLogin = RideSaver.Server.Models.UserLogin;
using InternalAPI;
using Grpc.Net.Client;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using AuthContext = AuthService.Data.AuthContext;

namespace AuthService.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AuthContext _authContext;
        public AuthenticationRepository(AuthContext authContext)
        {
            _authContext = authContext;
        }
        public async Task<bool> AuthenticateUserAsync(UserLogin userLogin) // TODO: Better user information authentication.
        {
            var userInfo = await _authContext.UserCredentials.SingleOrDefaultAsync(u => u.Username == userLogin.Username);

            if (userInfo is null) return false;
            if(userInfo.Username != userLogin.Username) return false;
            if (!Security.Argon2.VerifyHash(userLogin.Password, userInfo.PasswordHash!, userInfo.PasswordSalt!)) return false;
            return true;
        }

        public async Task<UserModel> GetUserAsync(string userName)
        {
            var userInfo = await _authContext.UserCredentials.FindAsync(userName);
            if (userInfo is not null) return userInfo;

            return null;
        }

        public async Task<bool> ValidateUserAsync(string userName)
        {
            var userInfo = await _authContext.UserCredentials.FindAsync(userName);
            if (userInfo is not null) return true;

            return false;
        }
    }
}