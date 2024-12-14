using Amazon;
using Amazon.S3;
using PFM.Website.Configurations;
using PFM.Website.Monitoring.Logging;
using PFM.Website.Monitoring.Metrics;
using PFM.Website.Monitoring.Tracing;
using PFM.Website.Persistence;
using PFM.Website.Persistence.Implementations;
using PFM.Website.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>() ?? new ApplicationSettings();

builder.Services.AddSingleton(appSettings);

if (appSettings.UseRemoteStorageForBankIcons)
{
    var s3Config = new AmazonS3Config() {
        RegionEndpoint = RegionEndpoint.GetBySystemName(appSettings.AwsRegion)
    };

    if (!string.IsNullOrEmpty(appSettings.AwsEndpointUrl))
    {
        s3Config.ServiceURL = appSettings.AwsEndpointUrl;
        s3Config.AuthenticationRegion = appSettings.AwsRegion;
        s3Config.ForcePathStyle = true;
    }

    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Config));

    builder.Services
        .AddSingleton<IObjectStorageService, AwsS3Service>();
}
else
{
    builder.Services
        .AddSingleton<IObjectStorageService, LocalStorageService>();
}

builder.Services
    .AddSingleton<ExpenseTypeService>()
    .AddSingleton<BankService>()
    .AddSingleton<CountryService>()
    .AddSingleton<BankAccountService>()
    .AddSingleton<CurrencyService>()
    .AddSingleton<IncomeService>()
    .AddSingleton<SavingService>()
    .AddSingleton<AtmWithdrawService>()
    .AddSingleton<ExpenseService>()
    .AddSingleton<PaymentMethodService>()
    .AddSingleton<MovementSummaryService>()
    .AddSingleton<BudgetPlanService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services
    .AddAuth(builder.Configuration)
    .AddObjectMapper()
    .ConfigureLogging(builder.Configuration, builder.Environment)
    .ConfigureTracing(appSettings.TracingOptions)
    .ConfigureMetrics(appSettings.MetricsOptions)
    .AddPfmApi(builder.Configuration, builder.Environment.EnvironmentName != "Production");

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

