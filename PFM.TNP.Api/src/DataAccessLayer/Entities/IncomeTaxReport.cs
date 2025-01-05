using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class IncomeTaxReport : PersistedEntity
{
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public int CurrencyId { get; set; } 
    
    [Required]
    public int CountryId { get; set; } 
    
    [Required]
    public decimal TaxableIncome { get; set; } 
    
    [Required]
    public decimal IncomeTaxPaid { get; set; } 
    
    [Required]
    public decimal NationalInsurancePaid { get; set; } 
    
    [Required]
    public DateTime PayDay  { get; set; }
}