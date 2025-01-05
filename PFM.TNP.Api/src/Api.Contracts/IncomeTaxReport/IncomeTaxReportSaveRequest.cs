using System;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.TNP.Api.Contracts.IncomeTaxReport;

public class IncomeTaxReportSaveRequest
{
    public int CurrencyId { get; set; } 
    
    public int CountryId { get; set; } 
    
    public decimal TaxableIncome { get; set; } 
    
    public decimal IncomeTaxPaid { get; set; } 
    
    public decimal NationalInsurancePaid { get; set; } 
    
    public DateOnly PayDay  { get; set; }
}