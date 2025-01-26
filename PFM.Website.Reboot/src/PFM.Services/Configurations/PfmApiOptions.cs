namespace PFM.Services.Configurations;

public class PfmApiOptions
{
    public bool Enabled { get; init; } = false;
    public string EndpointUrl { get; init; } = string.Empty;
}