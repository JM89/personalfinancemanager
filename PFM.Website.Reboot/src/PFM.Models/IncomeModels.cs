using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class IncomeListModel
{
    public int Id { get; set; }

    public string AccountCurrencySymbol { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public string Description { get; set; } = string.Empty;

    public string DisplayedCost
    {
        get
        {
            return AccountCurrencySymbol + String.Format("{0:0.00}", this.Cost);
        }
    }

    public DateTime DateIncome { get; set; }

    public string DisplayedDateIncome
    {
        get
        {
            return this.DateIncome.ToString("yyyy-MM-dd");
        }
    }
}
    
public class IncomeEditModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Required]
    [Range(0.00, 999999.99, ErrorMessage = "The field {0} must be positive.")]
    public decimal Cost { get; set; }

    [Required]
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime DateIncome { get; set; }
}