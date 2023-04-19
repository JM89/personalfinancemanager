using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        Task<bool> CreateAtmWithdraws(List<AtmWithdrawEditModel> atmWithdrawEditModel);

        Task<IList<AtmWithdrawListModel>> GetAtmWithdrawsByAccountId(int accountId);

        Task<bool> CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        Task<AtmWithdrawEditModel> GetById(int id);

        Task<bool> EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        Task<bool> CloseAtmWithdraw(int id);

        Task<bool> DeleteAtmWithdraw(int id);

        Task<bool> ChangeDebitStatus(int id, bool debitStatus);
    }
}