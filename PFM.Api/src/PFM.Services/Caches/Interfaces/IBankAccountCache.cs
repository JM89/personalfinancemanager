using PFM.Bank.Api.Contracts.Account;
using System.Threading.Tasks;

namespace PFM.Services.Caches.Interfaces
{

    public interface IBankAccountCache
    {
        Task<AccountDetails> GetById(int id);
    }
}
