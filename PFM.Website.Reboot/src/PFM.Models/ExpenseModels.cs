using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class ExpenseEditModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Required]
    public DateTime DateExpense { get; set; }

    [Required]
    public decimal Cost { get; set; }

    [Required]
    public int ExpenseTypeId { get; set; }

    [Required]
    public int PaymentMethodId { get; set; }

    [Required]
    public string Description { get; set; } = String.Empty;

    public int? AtmWithdrawId { get; set; }

    public int? TargetInternalAccountId { get; set; }

    public string DisplayedDateExpense { get; set; } = string.Empty;
    public bool HasBeenAlreadyDebited { get; set; }

    public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

    public int? GeneratedIncomeId { get; set; }
}

public class ExpenseListModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string AccountName { get; set; } = string.Empty;

    public string AccountCurrencySymbol { get; set; } = string.Empty;

    public DateTime DateExpense { get; set; }

    public decimal Cost { get; set; }

    public int ExpenseTypeId { get; set; }

    public string ExpenseTypeName { get; set; } = string.Empty;

    public int PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = string.Empty;

    public string PaymentMethodIconClass { get; set; } = string.Empty;

    public bool PaymentMethodHasBeenAlreadyDebitedOption { get; set; }

    public string Description { get; set; } = string.Empty;

    public string DisplayedCost => this.AccountCurrencySymbol + String.Format("{0:0.00}", this.Cost);

    public string DisplayedDateExpense => this.DateExpense.ToString("yyyy-MM-dd");

    public bool HasBeenAlreadyDebited { get; set; }

    public int? AtmWithdrawId { get; set; }

    public int? TargetInternalAccountId { get; set; }
}

public class ExpenseSearchParamModel
{
    public string UserId { get; set; } = string.Empty;

    public int? AccountId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? ExpenseTypeId { get; set; }

    public bool? ShowOnDashboard { get; set; }
}