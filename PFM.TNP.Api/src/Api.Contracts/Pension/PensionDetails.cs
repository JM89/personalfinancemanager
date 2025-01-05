using System;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.Pension.Api.Contracts.Pension;

public class PensionDetails
{
    public Guid Id { get; set; }
    
    public string SchemeName { get; set; }
    
    public string LoginUrl { get; set; }
    
    public string UserId { get; set; }
    
    public int CurrencyId { get; set; } 
    
    public int CountryId { get; set; } 
    
    public decimal CurrentPot { get; set; } 
    
    public decimal CurrentContribution { get; set; } 
    
    public DateTime LastUpdated  { get; set; }
}