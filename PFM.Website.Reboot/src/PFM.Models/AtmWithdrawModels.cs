﻿using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class AtmWithdrawListModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string AccountCurrencySymbol { get; set; } = string.Empty;

    public string Description => "ATM " + DisplayedDateExpense + " (Left: " + DisplayedCurrentAmount + ")";

    public DateTime DateExpense { get; set; }

    public string DisplayedDateExpense => this.DateExpense.ToString("yyyy-MM-dd");

    public decimal InitialAmount { get; set; }

    public bool IsClosed { get; set; }

    public string DisplayedInitialAmount => this.AccountCurrencySymbol + String.Format("{0:0.00}", this.InitialAmount);

    public decimal CurrentAmount { get; set; }

    public string DisplayedCurrentAmount => this.AccountCurrencySymbol + String.Format("{0:0.00}", this.CurrentAmount);

    public bool HasBeenAlreadyDebited { get; set; }

    public bool CanBeEdited { get; set; }

    public bool CanBeDeleted { get; set; }
}

public class AtmWithdrawEditModel
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    [Required]
    public DateTime DateExpense { get; set; }

    public string DisplayedDateExpense => this.DateExpense.ToString("yyyy-MM-dd");

    [Required]
    public decimal InitialAmount { get; set; }

    public decimal CurrentAmount { get; set; }

    public bool HasBeenAlreadyDebited { get; set; }
}