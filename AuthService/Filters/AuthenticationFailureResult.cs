using System.Net;
using System.Web.Http;

namespace AuthService.Filters
{
    // Filter that checks if the credentials are invalid, if so -> the context.ErrorResult is set to an IHttpActionResult which creates an error response.
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase { get; }
        public HttpRequestMessage Request { get; }
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase= reasonPhrase;
            Request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase
            };

            return response;
        }
    }
}
