using PFM.Services.Configurations;
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

builder.Services.AddExternalServices(appSettings.ExternalServiceSettings, builder.Environment.IsDevelopment());

builder.Services
    .AddAuth(builder.Configuration)
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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

