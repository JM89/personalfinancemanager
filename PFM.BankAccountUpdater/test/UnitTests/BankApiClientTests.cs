using System.Net;
using System.Text;
using System.Text.Json;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using PFM.Bank.Api.Contracts.Account;
using PFM.BankAccountUpdater.Extensions;
using PFM.BankAccountUpdater.ExternalServices.BankApi;
using PFM.BankAccountUpdater.Services.Interfaces;
using Xunit;

namespace UnitTests;

public class BankApiClientTests
{
    [Fact]
    public void Event_handler_registration_resolves_the_bank_api_client()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["BankApi:EndpointUrl"] = "https://bank.example/",
            ["AuthApi:EndpointUrl"] = "https://auth.example/"
        }).Build();
        var services = new ServiceCollection();
        services.AddSingleton<Serilog.ILogger>(Serilog.Log.Logger);
        services.AddMemoryCache();
        services.AddServices();
        services.AddBankApi(configuration, false);
        services.AddAuthenticationAndAuthorization(configuration);

        services.AddEventHandlers();

        using var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetRequiredService<IBankAccountApi>());
    }

    [Fact]
    public async Task Refit_client_constructs_get_and_edit_requests()
    {
        using var handler = new RecordingHandler();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["BankApi:EndpointUrl"] = "https://bank.example/",
            ["AuthApi:EndpointUrl"] = "https://auth.example/"
        }).Build();
        var services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddBankApi(configuration, false);
        services.AddAuthenticationAndAuthorization(configuration);
        var tokenStore = A.Fake<IAuthTokenStore>();
        A.CallTo(() => tokenStore.GetToken()).Returns("token-1");
        services.AddSingleton(tokenStore);
        services.ConfigureAll<HttpClientFactoryOptions>(options =>
            options.HttpMessageHandlerBuilderActions.Add(builder => builder.PrimaryHandler = handler));
        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IBankAccountApi>();

        var getResult = await client.Get(42);
        var editResult = await client.Edit(42, "user-1", new AccountDetails { CurrentBalance = 75m });

        Assert.NotNull(getResult.Data);
        Assert.NotNull(editResult.Data);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal((HttpMethod.Get, "/api/BankAccount/Get/42"), handler.Requests[0]);
        Assert.Equal((HttpMethod.Put, "/api/BankAccount/Edit/42/user-1"), handler.Requests[1]);
        Assert.All(handler.Authorizations, value => Assert.Equal("Bearer token-1", value));
        using var body = JsonDocument.Parse(handler.EditBody);
        Assert.Equal(75m, body.RootElement.GetProperty("currentBalance").GetDecimal());
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public List<(HttpMethod Method, string Path)> Requests { get; } = [];
        public List<string?> Authorizations { get; } = [];
        public string EditBody { get; private set; } = "";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add((request.Method, request.RequestUri!.AbsolutePath));
            Authorizations.Add(request.Headers.Authorization?.ToString());
            if (request.Content is not null)
                EditBody = await request.Content.ReadAsStringAsync(cancellationToken);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\":true}", Encoding.UTF8, "application/json")
            };
        }
    }
}
