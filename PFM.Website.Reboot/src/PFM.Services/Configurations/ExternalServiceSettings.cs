namespace PFM.Services.Configurations;

public class ExternalServiceSettings
{
    public BankIconSettings BankIconSettings { get; init; } = new();
    
    public PfmApiOptions PfmApiOptions { get; init; } = new ();
    
    public string AwsRegion { get; init; } = "eu-west-2";
    
    public string AwsEndpointUrl { get; init; } = string.Empty;
}

public class BankIconSettings
{
    public bool UseRemoteStorage { get; init; } = false;
    
    public string Location { get; init; } = string.Empty;
}

public class PfmApiOptions
{
    public bool Enabled { get; init; } = false;
    public string EndpointUrl { get; init; } = string.Empty;
}