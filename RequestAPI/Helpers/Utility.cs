using System.Net.Http.Headers;

namespace RequestAPI.Helpers
{
    public static class Utility
    {
        public static string GetAuthorizationToken(Microsoft.Extensions.Primitives.StringValues headers)
        {
            string? token = null;
            if (AuthenticationHeaderValue.TryParse(headers, out var headerValue))
            {
                token = headerValue.Parameter;
            }

            return token;
        }
    }
}
