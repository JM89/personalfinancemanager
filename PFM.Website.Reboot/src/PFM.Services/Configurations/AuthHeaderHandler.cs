using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

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
            var bearerToken = TokenProvider.AccessToken;

            if (bearerToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}

