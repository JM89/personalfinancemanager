using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.Saving;
using System;

namespace PersonalFinanceManager.Services.MovementStrategy
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

        public Movement(SavingEditModel saving)
        {
            this.Date = saving.DateSaving;
            this.Description = saving.Description;
            this.Amount = saving.Amount;
            this.PaymentMethod = PaymentMethod.InternalTransfer;
            this.SourceAccountId = saving.AccountId;
            this.TargetAccountId = saving.TargetInternalAccountId;
            this.TargetIncomeId = saving.GeneratedIncomeId;
        }

        public Movement(ExpenditureEditModel expenditure)
        {
            this.Date = expenditure.DateExpenditure;
            this.Description = expenditure.Description;
            this.Amount = expenditure.Cost;
            this.PaymentMethod = (PaymentMethod)expenditure.PaymentMethodId;
            this.SourceAccountId = expenditure.AccountId;
            this.TargetAccountId = expenditure.TargetInternalAccountId;
            this.TargetIncomeId = expenditure.GeneratedIncomeId;
            this.AtmWithdrawId = expenditure.AtmWithdrawId;
        }
    }
}
