using PFM.Services.Configurations;
using PFM.Services.Monitoring.Logging;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;
using PFM.Services.Persistence;
using PFM.Website.Configurations;
using PFM.Website.Offline;
using PFM.Website.Monitoring.Metrics;
using PFM.Website.Monitoring.Tracing;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();
var offline = builder.Configuration.GetValue<bool>("AuthOptions:DevOffline");
if (offline && (appSettings.ExternalServiceSettings.PfmApiOptions.Enabled || appSettings.ExternalServiceSettings.BankIconSettings.UseRemoteStorage))
    throw new InvalidOperationException("Offline mode requires the in-memory API and local bank icon storage.");

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
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!offline)
    app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (offline)
{
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/api/frontend") &&
            !HttpMethods.IsGet(context.Request.Method) &&
            context.Request.Headers.TryGetValue("Origin", out var origin) &&
            (!Uri.TryCreate(origin.ToString(), UriKind.Absolute, out var uri) ||
             (uri.Host != "localhost" && uri.Host != "127.0.0.1") ||
             (uri.Port != 5174 && uri.Port != 5110 && uri.Port != 7142)))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        await next();
    });
}

app.MapRazorPages();
app.MapBlazorHub();
if (offline)
{
    app.MapGet("/health/frontend", () => Results.Ok(new { status = "ready" }));
    app.MapOfflineEndpoints();
}
else
    app.MapFallbackToPage("/_Host");

app.Run();
