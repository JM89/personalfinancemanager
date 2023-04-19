using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankService : IBaseService
    {
        Task<IList<BankListModel>> GetBanks();

        Task<bool> CreateBank(BankEditModel bankEditModel);

        Task<BankEditModel> GetById(int id);

        Task<bool> EditBank(BankEditModel bankEditModel);

        Task<bool> DeleteBank(int id);
    }
}