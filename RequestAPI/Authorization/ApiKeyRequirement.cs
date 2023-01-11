using Microsoft.AspNetCore.Authorization;

namespace RequestAPI.Authorization
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public string? PolicyName { get; }
    }
}
