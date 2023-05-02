using PFM.Bank.Api.Contracts.Bank;
using System.Threading.Tasks;

namespace PFM.Services.Caches.Interfaces
{
    public interface IBankCache
    {
        Task<BankDetails> GetById(int id);
    }
}
