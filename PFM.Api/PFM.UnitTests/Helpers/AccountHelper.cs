using PFM.DataAccessLayer.Entities;

namespace PFM.UnitTests.Helpers
{
    public static class AccountHelper
    {
        public static Account CreateAccountModel(int accountId)
        {
            var account = new Account()
            {
                Id = accountId, 
                Bank = BankHelper.CreateBankModel(),
                Currency = CurrencyHelper.CreateCurrencyModel()
            };
            return account;
        }
    }
}
