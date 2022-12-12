using DataAccess.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using InternalAPI;
using RideSaver.Server.Models;
using UserService.Data;
using UserService.Repository;

namespace UserService.AuthServices
{
    public class AuthService : Authentication.AuthenticationBase , IAuthService
    {
        private readonly Authentication.AuthenticationClient _authClient;
        private readonly UserContext _userContext;
        public AuthService(Authentication.AuthenticationClient authClient, UserContext userContext)
        {
            _userContext = userContext;
            _authClient = authClient;
        }
        public override async Task<IdentityResponse> GetIdentity(IdentityRequest request, ServerCallContext context)
        {
            if (request is null) return null;
            var userName = request.Username;
            var userInfo =  await _userContext.Users.FindAsync(userName);
            if (userInfo is null) return null;
            var response = new IdentityResponse()
            {
                Id = userInfo.Id.ToString(),
                Username = userInfo.Username,
                PasswordSalt = BytesToString(userInfo.PasswordSalt!),
                PasswordHash = BytesToString(userInfo.PasswordHash!),
                Email = userInfo.Email,
            };

            return response;
        }

        public async Task<Empty> PostIdentityToAuthService(UserModel model)
        {
            var userModel = new IdentityResponse()
            {
                Id = model.Id.ToString(),
                Username = model.Username,
                PasswordSalt = BytesToString(model.PasswordSalt!),
                PasswordHash = BytesToString(model.PasswordHash!),
                Email = model.Email
            };

            await _authClient.AddIdentityAsync(userModel);
            return new Empty();
        }

        static string BytesToString(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
