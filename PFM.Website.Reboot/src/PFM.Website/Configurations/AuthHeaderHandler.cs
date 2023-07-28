using System;
using System.Net.Http.Headers;

namespace PFM.Website.Configurations
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearerToken = _httpContextAccessor?.HttpContext?.Request.Headers.Authorization.FirstOrDefault() ?? null;

            if (bearerToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}

