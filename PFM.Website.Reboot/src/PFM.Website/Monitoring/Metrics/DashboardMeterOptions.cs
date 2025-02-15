using System.Diagnostics.Metrics;
using PFM.Services.Utils;

namespace PFM.Website.Monitoring.Metrics;

public class DashboardMeterOptions: MeterOptions
{
    public DashboardMeterOptions(string name): base(name.ToSnakeCase())
    {
        Tags = new List<KeyValuePair<string, object?>>()
        {
            { new("unit", "items") },
            { new("context", "pfm.website") }
        };
    }
}