using DataAccess.Models;
using Grpc.Core;
using IdentityService.Controllers;
using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;
using IdentityService.Data;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserContext _userContext;

        public AuthService(ITokenService tokenService, UserContext userContext)
        {
            _tokenService = tokenService;
            _userContext = userContext;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var userInfo = await _userContext.Users.FirstAsync(u => u.Username == model.Username);

            if (userInfo is null) return null;
            if (userInfo.Username != model.Username) return null;
            if (!Security.Argon2.VerifyHash(model.Password!, userInfo.PasswordHash!, userInfo.PasswordSalt!)) return null;

            var jwtToken = _tokenService.GenerateToken(userInfo);
            var refreshToken = _tokenService.GenerateRefreshToken();

            userInfo.RefreshTokens!.Add(refreshToken);
            _userContext.Update(userInfo);
            await _userContext.SaveChangesAsync();

            return new AuthenticateResponse(userInfo, jwtToken, refreshToken.Token!);
        }
        public async Task<AuthenticateResponse> RefreshToken(string token)
        {
            var userInfo = await _userContext.Users.FirstAsync(u => u.RefreshTokens!.Any(t => t.Token== token));

            if (userInfo is null) return null;

            var refreshToken = userInfo.RefreshTokens?.Single(x => x.Token== token);

            if (!refreshToken!.IsActive) return null;

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            userInfo.RefreshTokens!.Add(newRefreshToken);

            _userContext.Update(userInfo);
            await _userContext.SaveChangesAsync();

            var jwtToken = _tokenService.GenerateToken(userInfo);
            return new AuthenticateResponse(userInfo, jwtToken, newRefreshToken.Token!);
            
        }
        public async Task<bool> RevokeToken(string token)
        {
            var userInfo = await _userContext.Users.FirstAsync(u => u.RefreshTokens!.Any(t => t.Token == token));

            if(userInfo is null) return false;

            var refreshToken = userInfo.RefreshTokens!.Single(x => x.Token== token);

            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            _userContext.Update(userInfo);
            await _userContext.SaveChangesAsync();

            return true;
        }
    }
}
