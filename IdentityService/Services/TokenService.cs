using DataAccess.DataModels;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration; // appsettings.json configuration
        //private readonly TimeSpan ExpiryDuration = new(2, 0, 0); // 2 hours expiry-duration
        private readonly ILogger _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateToken(UserModel userInfo)
        {
            var Claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, value: userInfo.Username!.ToString()),
                new Claim(ClaimTypes.Email, value: userInfo.Email!.ToString()),
                new Claim(ClaimTypes.NameIdentifier , value: userInfo.Id.ToString()) // GUID 
            });

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptionKey = _configuration["Jwt:Key"];
            var key = Encoding.UTF8.GetBytes(encryptionKey!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Subject = Claims,
                //Expires = DateTime.Now + ExpiryDuration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<bool> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptionKey = _configuration["Jwt:Key"];
            var key = Encoding.UTF8.GetBytes(encryptionKey!);
            var validation = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            });

            if (!validation.IsValid) return false;

            return true;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var RNG = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            RNG.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken) return null;

                var symmetricKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
                var validationParameters = new TokenValidationParameters()
                {
                    //RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                return principal;
            }

            catch (Exception ex)
            {
                _logger.LogInformation("[TokenService::GetPrincipal::Exception] " + ex.Message);
                return null;
            }
        }
    }
}
