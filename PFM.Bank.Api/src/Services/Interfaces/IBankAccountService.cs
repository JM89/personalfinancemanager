using PFM.Bank.Api.Contracts.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        Task<bool> CreateBankAccount(AccountDetails accountDetails, string userId);

        Task<List<AccountList>> GetAccountsByUser(string userId);

        Task<AccountDetails> GetById(int id);

        Task<bool> EditBankAccount(AccountDetails accountDetails, string userId);

        Task<bool> DeleteBankAccount(int id);

        Task<bool> SetAsFavorite(int id);
    }
}