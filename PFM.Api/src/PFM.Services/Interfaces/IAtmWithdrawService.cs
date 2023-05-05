using PFM.Api.Contracts.AtmWithdraw;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        Task<bool> CreateAtmWithdraws(List<AtmWithdrawDetails> atmWithdrawDetails);

        Task<IList<AtmWithdrawList>> GetAtmWithdrawsByAccountId(int accountId);

        Task<bool> CreateAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails);

        Task<AtmWithdrawDetails> GetById(int id);

        Task<bool> CloseAtmWithdraw(int id);

        Task<bool> DeleteAtmWithdraw(int id);

        Task<bool> ChangeDebitStatus(int id, bool debitStatus);
    }
}