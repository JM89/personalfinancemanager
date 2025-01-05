using System;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.TNP.Api.Contracts.Pension;

public class PensionSaveRequest
{
    public string SchemeName { get; set; }
    
    public string LoginUrl { get; set; }
    
    public int CurrencyId { get; set; } 
    
    public int CountryId { get; set; } 
    
    public decimal CurrentPot { get; set; } 
    
    public decimal CurrentContribution { get; set; }
}