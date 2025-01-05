using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class Pension : PersistedEntity
{
    [Required]
    public string SchemeName { get; set; }
    
    [Required]
    public string LoginUrl { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public int CurrencyId { get; set; } 
    
    [Required]
    public int CountryId { get; set; } 
    
    [Required]
    public decimal CurrentPot { get; set; } 
    
    [Required]
    public decimal CurrentContribution { get; set; } 
    
    [Required]
    public DateTime LastUpdated  { get; set; }
}