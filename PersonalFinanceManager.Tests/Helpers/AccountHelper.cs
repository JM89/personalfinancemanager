using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.BudgetPlan;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class AccountHelper
    {
        public static AccountModel CreateAccountModel(int accountId)
        {
            var account = new AccountModel()
            {
                Id = accountId, 
                Bank = BankHelper.CreateBankModel(),
                Currency = CurrencyHelper.CreateCurrencyModel()
            };
            return account;
        }
    }
}
