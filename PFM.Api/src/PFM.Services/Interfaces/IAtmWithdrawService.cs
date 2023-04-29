using PFM.Api.Contracts.AtmWithdraw;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        Task<bool> CreateAtmWithdraws(List<AtmWithdrawDetails> atmWithdrawDetails);

        IList<AtmWithdrawList> GetAtmWithdrawsByAccountId(int accountId);

        Task<bool> CreateAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails);

        AtmWithdrawDetails GetById(int id);

        Task<bool> EditAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails);

        void CloseAtmWithdraw(int id);

        Task<bool> DeleteAtmWithdraw(int id);

        void ChangeDebitStatus(int id, bool debitStatus);
    }
}