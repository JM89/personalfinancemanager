using System.Net.Http.Headers;

namespace PFM.Api.Extensions
{
    public class AuthHeaderHandler(IHttpContextAccessor httpContextAccessor, Serilog.ILogger logger) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = httpContextAccessor.HttpContext ?? new DefaultHttpContext();
            var bearerToken = context.Request.Headers.Authorization.FirstOrDefault() ?? null;

            if (bearerToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
