using PFM.BankAccountUpdater.Services.Interfaces;
using System.Net.Http.Headers;

namespace PFM.BankAccountUpdater.Extensions
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthTokenStore _authTokenStore;

        public AuthHeaderHandler(IAuthTokenStore authTokenStore)
        {
            _authTokenStore = authTokenStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var bearerToken = await _authTokenStore.GetToken();

            if (bearerToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
