using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class SavingEditModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Required]
    public DateTime DateSaving { get; set; }

    [Required]
    [Range(0.00, 999999.99, ErrorMessage = "The field {0} must be positive.")]
    public decimal Amount { get; set; }

    public int? TargetInternalAccountId { get; set; }

    public string DisplayedDateSaving { get; set; } = string.Empty;

    public string Description => "Saving " + DisplayedDateSaving;

    public int? GeneratedIncomeId { get; set; }
}

public class SavingListModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string AccountName { get; set; } = string.Empty;

    public string AccountCurrencySymbol { get; set; } = string.Empty;

    public DateTime DateSaving { get; set; } = DateTime.UtcNow;

    public decimal Amount { get; set; } 

    public string DisplayedAmount { get; set; } = string.Empty;

    public string DisplayedDateSaving { get; set; } = string.Empty;

    public string Description => "Saving " + DisplayedDateSaving;

    public int TargetInternalAccountId { get; set; }

    public string TargetInternalAccountName { get; set; } = string.Empty;
}

