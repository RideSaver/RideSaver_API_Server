using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        private readonly TimeSpan ExpiryDuration = new TimeSpan(2, 0, 0);

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Generates the key based on the appsettings JSON file secret
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // generates the creds by hashing the key

            var claims = new List<Claim> // creates a claim to add the user model properties to the token
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Name),
                new Claim("UserCreationDate", userInfo.CreatedAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.UserId),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims, DateTime.Now + ExpiryDuration, signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}
