using System.Net.Http.Headers;
using System.Web.Http;

namespace AuthService.Filters
{
    // Authentication Challenge filter to the request-headers for unauthorized requests
    public class ChallengeUnauthorizedResult : IHttpActionResult
    {
        public AuthenticationHeaderValue Challenge { get; }
        public IHttpActionResult InnerResult { get; }
        public ChallengeUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if(response.Headers.WwwAuthenticate.All(h => h.Scheme != Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
}
