using System.IO;
using System.Text.Json;
using Api.Extensions;
using Api.Settings;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

namespace UnitTests.Api;

public class SwaggerSecurityTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SwaggerDocument_ReflectsAuthenticationSetting(bool enabled)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMvcCore().AddApiExplorer();
        services.AddSingleton(A.Fake<IWebHostEnvironment>());
        services.AddSwaggerDefinition(new ApplicationSettings { AuthOptions = new AuthOptions { Enabled = enabled } });
        using var provider = services.BuildServiceProvider();
        var document = provider.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");
        using var writer = new StringWriter();
        document.SerializeAsV3(new OpenApiJsonWriter(writer));
        using var json = JsonDocument.Parse(writer.ToString());
        var root = json.RootElement;

        if (enabled)
        {
            var bearer = root.GetProperty("components").GetProperty("securitySchemes").GetProperty("Bearer");
            Assert.Equal("apiKey", bearer.GetProperty("type").GetString());
            Assert.Equal("Authorization", bearer.GetProperty("name").GetString());
            Assert.Equal("header", bearer.GetProperty("in").GetString());
            Assert.Equal(0, root.GetProperty("security")[0].GetProperty("Bearer").GetArrayLength());
        }
        else
        {
            Assert.False(root.TryGetProperty("security", out _));
        }
    }
}
