using PFM.Api.Configurations.Monitoring.Metrics;
using PFM.Api.Configurations.Monitoring.Tracing;
using PFM.Services.ExternalServices.TaxAndPensionApi;

namespace PFM.Api.Settings;

public class ApplicationSettings
{    
    public string ApplicationName { get; set; } = nameof(Api);
    public string ShortDescription { get; set; } = $"This is {nameof(Api)}";
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
    public AuthOptions AuthOptions { get; set; } = new ();
    
    public ApiOptions BankApiOptions { get; set; } = new ();
    public ApiOptions TaxAndPensionApiOptions { get; set; } = new ();
}