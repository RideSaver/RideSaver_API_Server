using Microsoft.AspNetCore.Authorization;

namespace EstimateAPI.Authorization
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public string PolicyName { get; }
    }
}
