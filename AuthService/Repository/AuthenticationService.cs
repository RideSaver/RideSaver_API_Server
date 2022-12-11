using AuthService.Data;
using DataAccess.Models;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using InternalAPI;
using NetTopologySuite.IO;
using System.Linq;
using System.Text;
using AuthContext = AuthService.Data.AuthContext;

namespace AuthService.Repository
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        private readonly Authentication.AuthenticationClient _authClient;
        private readonly AuthContext _authContext;
        private readonly GrpcChannel _channel;
        public AuthenticationService(Authentication.AuthenticationClient authClient, AuthContext authContext)
        {
            _authContext = authContext;
            _authClient = authClient;
            _channel = GrpcChannel.ForAddress($"https://user.api");
        }
        public async Task<UserModel> GetUserIdentityAsync(string userName)
        {
            var client = new Authentication.AuthenticationClient(_channel);
            var response = await client.GetIdentityAsync(new IdentityRequest()
            {
                Username = userName
            });

            if (response is null) return null;
            var userInfo = new UserModel()
            {
                Id = new Guid(response.Id),
                Username = response.Username,
                Email = response.Email,
                PasswordSalt = Encoding.ASCII.GetBytes(response.PasswordSalt),
                PasswordHash = Encoding.ASCII.GetBytes(response.PasswordHash),
            };

            return userInfo;
        }

        public override async Task<Empty> AddIdentity(IdentityResponse response, ServerCallContext context)
        {
            if (response is null) return null;
            var userInfo = new UserModel()
            {
                Id = new Guid(response.Id),
                Username = response.Username,
                Email = response.Email,
                PasswordSalt = Encoding.ASCII.GetBytes(response.PasswordSalt),
                PasswordHash = Encoding.ASCII.GetBytes(response.PasswordHash),
            };

            await _authContext.UserCredentials.AddAsync(userInfo);
            return new Empty();
        }
    }
}
