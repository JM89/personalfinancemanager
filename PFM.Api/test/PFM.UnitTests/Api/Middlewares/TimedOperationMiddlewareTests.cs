using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using PFM.Api.Configurations.Monitoring.Metrics;
using PFM.Api.MiddleWares;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace PFM.UnitTests.Api.MiddleWares;

public class TimedOperationMiddlewareTests
{
    private readonly TimedOperationMiddleware _sut;

    public TimedOperationMiddlewareTests()
    {
        var next = A.Fake<RequestDelegate>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.InMemory()
            .CreateLogger();
        _sut = new TimedOperationMiddleware(next, new RequestMetrics());
    }
    
    [Fact]
    public async Task WhenValidContext_ShouldExecuteInvokeAsyncSuccessfully_AndOperationIsMonitored()
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request = { Path = "/api/test" }
        };
        var exportedItems = new List<Metric>();
        
        // Act
        using (MeterProvider(exportedItems))
        {
            await _sut.InvokeAsync(httpContext);
        }
        
        // Assert
        InMemorySink.Instance.Should()
            .HaveMessage(" /api/test {Outcome} in {Elapsed:0.0} ms")
            .Appearing().Once()
            .WithProperty("Outcome").WithValue("completed");
        Assert.True(exportedItems.Any(x => x.Name == "http_request_counter"), "http_request_counter should exist");
        Assert.True(exportedItems.Any(x => x.Name == "http_request_timer"), "http_request_timer should exist");
    }
    
    [Fact]
    public async Task WhenValidContextWithIgnoredPath_ShouldExecuteInvokeAsyncSuccessfully_AndOperationIsNotMonitored()
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request = { Path = "/swagger" }
        };
        var exportedItems = new List<Metric>();

        // Act
        using (MeterProvider(exportedItems))
        {
            await _sut.InvokeAsync(httpContext);
        }
        
        // Assert
        InMemorySink.Instance.Should().NotHaveMessage();
        Assert.False(exportedItems.Any());
    }
    
    private MeterProvider MeterProvider(List<Metric> exportedItems)
    {
        return Sdk.CreateMeterProviderBuilder()
            .AddMeter(RequestMetrics.MeterName)
            .AddInMemoryExporter(exportedItems)
            .Build();
    }
}