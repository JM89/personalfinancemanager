using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PFM.CommonLibraries.Api.MiddleWares;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace PFM.CommonLibraries.Api.Tests.MiddleWares;

public class ResponseWrapperMiddlewareTests
{
    private readonly ResponseWrapperMiddleware _sut;

    public ResponseWrapperMiddlewareTests()
    {
        var next = A.Fake<RequestDelegate>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.InMemory()
            .CreateLogger();
        _sut = new ResponseWrapperMiddleware(next, Log.Logger);
    }
    
    [Fact]
    public async Task WhenValidContext_ShouldExecuteInvokeAsyncSuccessfully()
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request = { Path = "/api/test" },
            Response =
            {
                Body = CreateTestObject(Guid.NewGuid())
            }
        };

        // Act
        await _sut.InvokeAsync(httpContext);
        
        // Assert
        InMemorySink.Instance.Should().NotHaveMessage();
    }

    private class MyResponse
    {
        public Guid Value { get; set; }
    }
    
    private static MemoryStream CreateTestObject(Guid val)
    {
        var obj = new MyResponse { Value = val };
        return new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj)));
    }
}