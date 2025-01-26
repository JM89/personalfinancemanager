using PFM.Services.DependencyInjection;
using PFM.Services.Monitoring.Logging;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;
using PFM.Services.Persistence;
using PFM.Website.Configurations;
using PFM.Website.Monitoring.Metrics;
using PFM.Website.Monitoring.Tracing;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();

builder.Services.AddSingleton(appSettings);

if (appSettings.BankIconSettings.UseRemoteStorage)
{
    builder.Services.AddRemoteStorage(appSettings.AwsEndpointUrl, appSettings.AwsRegion);
}
else
{
    builder.Services.AddLocalStorage();
}

builder.Services.AddInternalServices();
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services
    .AddAuth(builder.Configuration)
    .AddObjectMapper()
    .ConfigureLogging(builder.Configuration, builder.Environment)
    .ConfigureWebsiteTracing(appSettings.TracingOptions)
    .ConfigureWebsiteMetrics(appSettings.MetricsOptions)
    .AddPfmApi(builder.Configuration, appSettings.PfmApiOptions, builder.Environment.IsDevelopment());

var app = builder.Build();

Log.Logger
    .ForContext("PfmApiOptions.Enabled", appSettings.PfmApiOptions.Enabled)
    .ForContext("PfmApiOptions.EndpointUrl", appSettings.PfmApiOptions.EndpointUrl)
    .Information("PFM Website started");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

