using System.Collections.Generic;
using System.Diagnostics.Metrics;
using PFM.Services.Utils;

namespace PFM.Services.Monitoring.Metrics;

public class ExampleMeterOptions: MeterOptions
{
    public ExampleMeterOptions(string name): base(name.ToSnakeCase())
    {
        Tags = new List<KeyValuePair<string, object>>()
        {
            { new("unit", "items") },
            { new("context", "pfm.api") }
        };
    }
}