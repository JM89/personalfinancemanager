using System;
using Api.Contracts.Shared;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.TNP.Api.Contracts.IncomeTaxReport;

public class IncomeTaxReportList : ICanBeDeleted
{
    public bool CanBeDeleted { get; set; }

    public string TooltipResourceName => "CantBeDeleted";
    
    public Guid Id { get; set; }
    
    public decimal TaxableIncome { get; set; } 
    
    public decimal IncomeTaxPaid { get; set; } 
    
    public decimal NationalInsurancePaid { get; set; } 
    
    public DateOnly PayDay  { get; set; }
}