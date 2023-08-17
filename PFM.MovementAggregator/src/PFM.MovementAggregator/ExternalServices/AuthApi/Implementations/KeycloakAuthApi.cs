using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PFM.MovementAggregator.ExternalServices.AuthApi.Contracts;

namespace PFM.MovementAggregator.ExternalServices.AuthApi.Implementations
{
    public class KeycloakAuthApi: IAuthApi
    {
        private readonly HttpClient _client;
        private readonly Serilog.ILogger _logger;
        private readonly JsonSerializerSettings _jsonOptions;

        public KeycloakAuthApi(Serilog.ILogger logger, HttpClient client)
		{
            _client = client;
            _logger = logger;
            _jsonOptions = new JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        public async Task<ClientToken> Login(ClientInfo info)
        {
            try
            {
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string>() {
                        { "client_id", info.ClientId },
                        { "client_secret", info.ClientSecret },
                        { "scope", "email" },
                        { "grant_type", "client_credentials" }
                });

                var response = await _client.PostAsync("", content);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ClientToken>(responseContent, _jsonOptions) ?? new ClientToken();
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Exception when calling Keycloak API");
                throw;
            }
        }
    }
}

