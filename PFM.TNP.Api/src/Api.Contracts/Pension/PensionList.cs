using System;
using PFM.TNP.Api.Contracts.Shared;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.TNP.Api.Contracts.Pension;

public class PensionList : ICanBeDeleted
{
    public bool CanBeDeleted { get; set; }

    public string TooltipResourceName => "CantBeDeleted";
    
    public Guid Id { get; set; }
    
    public string SchemeName { get; set; }
    
    public decimal CurrentPot { get; set; } 
    
    public decimal CurrentContribution { get; set; } 
    
    public DateTime LastUpdated  { get; set; }
}