using PFM.Bank.Api.Contracts.Bank;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBankService : IBaseService
    {
        Task<List<BankList>> GetBanks(string userId);

        Task<bool> CreateBank(BankDetails bankDetails, string userId);

        Task<BankDetails> GetById(int id);

        Task<bool> EditBank(BankDetails bankDetails, string userId);

        Task<bool> DeleteBank(int id);
    }
}