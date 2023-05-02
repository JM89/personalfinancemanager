using PFM.Bank.Api.Contracts.Bank;
using System.Collections.Generic;

namespace PFM.Services.Interfaces
{
    public interface IBankService : IBaseService
    {
        IList<BankList> GetBanks();

        void CreateBank(BankDetails bankDetails);

        BankDetails GetById(int id);

        void EditBank(BankDetails bankDetails);

        void DeleteBank(int id);
    }
}