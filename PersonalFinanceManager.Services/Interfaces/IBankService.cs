using System.Collections.Generic;
using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankService : IBaseService
    {
        IList<BankListModel> GetBanks();

        void CreateBank(BankEditModel bankEditModel);

        BankEditModel GetById(int id);

        void EditBank(BankEditModel bankEditModel);

        void DeleteBank(int id);
    }
}