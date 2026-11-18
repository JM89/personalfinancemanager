using Microsoft.Extensions.FileProviders;
using PFM.Services.Configurations;
using PFM.Services.Monitoring.Logging;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;
using PFM.Services.Persistence;
using PFM.Website.Configurations;
using PFM.Website.Frontend;
using PFM.Website.Monitoring.Metrics;
using PFM.Website.Monitoring.Tracing;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

builder.Services.AddRazorPages();
// Registers the standard JSON error format used by the production exception handler.
builder.Services.AddProblemDetails();

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();
var offline = builder.Configuration.GetValue<bool>("AuthOptions:DevOffline");
if (offline && (appSettings.ExternalServiceSettings.PfmApiOptions.Enabled || appSettings.ExternalServiceSettings.BankIconSettings.UseRemoteStorage))
    throw new InvalidOperationException("Offline mode requires the in-memory API and local bank icon storage.");
if (!offline && !appSettings.ExternalServiceSettings.PfmApiOptions.Enabled)
    throw new InvalidOperationException("Live mode requires the PFM API.");

if (builder.Environment.IsDevelopment())
{
    var reactDist = Path.GetFullPath("../PFM.Website.Client/dist", builder.Environment.ContentRootPath);
    if (Directory.Exists(reactDist))
        builder.Environment.WebRootFileProvider = new CompositeFileProvider(
            builder.Environment.WebRootFileProvider, new PhysicalFileProvider(reactDist));
}

builder.Services.AddSingleton(appSettings);

builder.Services.AddExternalServices(appSettings.ExternalServiceSettings, builder.Environment.IsDevelopment());

builder.Services
    .AddAuth(builder.Configuration, builder.Environment)
    .AddObjectMapper()
    .ConfigureLogging(builder.Configuration, builder.Environment)
    .ConfigureWebsiteTracing(appSettings.TracingOptions)
    .ConfigureWebsiteMetrics(appSettings.MetricsOptions);

var app = builder.Build();

Log.Logger.Information("PFM Website started");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!offline)
    app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/frontend"))
    {
        context.Response.Headers.CacheControl = "no-store";
        if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsHead(context.Request.Method) &&
            (!offline && context.Request.Headers["X-PFM-Frontend"] != "1" ||
             offline && context.Request.Headers.TryGetValue("Origin", out var origin) &&
             (!Uri.TryCreate(origin.ToString(), UriKind.Absolute, out var uri) ||
              (uri.Host != "localhost" && uri.Host != "127.0.0.1") ||
              !appSettings.OfflineAllowedOriginPorts.Contains(uri.Port))))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
    }
    await next();
});

app.MapRazorPages();
app.MapGet("/health/frontend", () => Results.Ok(new { status = "ready" }));
app.MapFrontendEndpoints(offline);
app.MapFallback("/api/{**path}", () => Results.NotFound());
app.MapFallbackToFile("index.html");

app.Run();
