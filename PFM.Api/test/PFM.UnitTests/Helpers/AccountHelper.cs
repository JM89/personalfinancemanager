using PFM.Bank.Api.Contracts.Account;

namespace PFM.UnitTests.Helpers
{
    public static class AccountHelper
    {
        public static AccountDetails CreateAccountModel(int accountId)
        {
            var account = new AccountDetails()
            {
                Id = accountId, 
                BankId = 1,
                CurrencyId = 1
            };
            return account;
        }
    }
}
