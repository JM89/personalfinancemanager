using System.Threading.Tasks;
using App.Metrics;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using PFM.CommonLibraries.Api.MiddleWares;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace PFM.CommonLibraries.Api.Tests.MiddleWares;

public class TimedOperationMiddlewareTests
{
    private readonly TimedOperationMiddleware _sut;

    public TimedOperationMiddlewareTests()
    {
        var metrics = A.Fake<IMetrics>();
        var next = A.Fake<RequestDelegate>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.InMemory()
            .CreateLogger();
        _sut = new TimedOperationMiddleware(next, metrics);
    }
    
    [Fact]
    public async Task WhenValidContext_ShouldExecuteInvokeAsyncSuccessfully_AndOperationIsLogged()
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request = { Path = "/api/test" }
        };

        // Act
        await _sut.InvokeAsync(httpContext);
        
        // Assert
        InMemorySink.Instance.Should()
            .HaveMessage(" /api/test {Outcome} in {Elapsed:0.0} ms")
            .Appearing().Once()
            .WithProperty("Outcome").WithValue("completed");
    }
    
    [Fact]
    public async Task WhenValidContextWithIgnoredPath_ShouldExecuteInvokeAsyncSuccessfully_AndOperationIsNotLogged()
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request = { Path = "/swagger" }
        };

        // Act
        await _sut.InvokeAsync(httpContext);
        
        // Assert
        InMemorySink.Instance.Should().NotHaveMessage();
    }
}