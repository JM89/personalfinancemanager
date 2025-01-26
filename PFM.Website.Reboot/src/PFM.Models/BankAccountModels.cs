using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class BankAccountListModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
        
    public string BankIconPath { get; set; } = string.Empty;
    public string CurrencyName { get; set; } = string.Empty;

    public string CurrencySymbol { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; }
    public string DisplayedInitialBalance{ get; set; } = string.Empty;

    public decimal CurrentBalance { get; set; }
    public string DisplayedCurrentBalance { get; set; } = string.Empty;
    public bool CanBeDeleted { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsSavingAccount { get; set; }
}

public class BankAccountEditModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int BankId { get; set; }

    [Required]
    public int CurrencyId { get; set; }

    public string CurrencyName { get; set; } = string.Empty; 

    public string CurrencySymbol { get; set; } = string.Empty;

    [Required]
    public decimal InitialBalance { get; set; }

    public decimal CurrentBalance { get; set; }

    public string DisplayedCurrentBalance { get; set; } = string.Empty;
    public bool IsSavingAccount { get; set; }

}