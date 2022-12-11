using DataAccess.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using InternalAPI;
using Microsoft.AspNetCore.Identity;
using UserService.Data;

namespace UserService.Repository
{
    public class UserService : Authentication.AuthenticationBase
    {
        private readonly Authentication.AuthenticationClient _authClient;
        private readonly IUserRepository _userRepository;
        private readonly UserContext _userContext;
        private readonly GrpcChannel _channel;
        public UserService(Authentication.AuthenticationClient authClient, UserContext userContext, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _authClient = authClient;
            _channel = GrpcChannel.ForAddress($"http://authentication.api");
        }
        public override async Task<IdentityResponse> GetIdentity(IdentityRequest request, ServerCallContext context)
        {
            if (request is null) return null;
            var userName = request.Username;
            var userInfo = await _userRepository.GetUserModelAsync(userName);
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
            var client = new Authentication.AuthenticationClient(_channel);

            var userModel = new IdentityResponse()
            {
                Id = model.Id.ToString(),
                Username = model.Username,
                PasswordSalt = BytesToString(model.PasswordSalt!),
                PasswordHash = BytesToString(model.PasswordHash!),
                Email = model.Email
            };

            await client.AddIdentityAsync(userModel);
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
