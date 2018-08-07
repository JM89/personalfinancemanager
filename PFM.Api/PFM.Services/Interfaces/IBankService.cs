using System.Collections.Generic;
using PFM.Services.Core;
using PFM.DTOs.Bank;

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