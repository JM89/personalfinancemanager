using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.Income;
using PFM.Api.Contracts.Saving;
using PFM.DataAccessLayer.Enumerations;
using System;

namespace PFM.Services.MovementStrategy
{
    public class Movement
    {
        public PaymentMethod PaymentMethod { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int? SourceAccountId { get; set; }

        public int? TargetAccountId { get; set; }

        public int? AtmWithdrawId { get; set; }

        public int? TargetIncomeId { get; set; }

        public Movement(SavingDetails saving)
        { 
            this.Date = saving.DateSaving;
            this.Description = saving.Description;
            this.Amount = saving.Amount;
            this.PaymentMethod = PaymentMethod.InternalTransfer;
            this.SourceAccountId = saving.AccountId;
            this.TargetAccountId = saving.TargetInternalAccountId;
            this.TargetIncomeId = saving.GeneratedIncomeId;
        }

        public Movement(ExpenseDetails expenditure)
        {
            this.Date = expenditure.DateExpense;
            this.Description = expenditure.Description;
            this.Amount = expenditure.Cost;
            this.PaymentMethod = (PaymentMethod)expenditure.PaymentMethodId;
            this.SourceAccountId = expenditure.AccountId;
            this.TargetAccountId = expenditure.TargetInternalAccountId;
            this.TargetIncomeId = expenditure.GeneratedIncomeId;
            this.AtmWithdrawId = expenditure.AtmWithdrawId;
        }

        public Movement(IncomeDetails income)
        {
            this.Date = income.DateIncome;
            this.Description = income.Description;
            this.Amount = income.Cost;
            this.PaymentMethod = PaymentMethod.Transfer;
            this.SourceAccountId = income.AccountId;
        }
    }
}
