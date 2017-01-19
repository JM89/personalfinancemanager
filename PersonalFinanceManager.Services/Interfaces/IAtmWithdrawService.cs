using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId);

        void CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        AtmWithdrawEditModel GetById(int id);

        void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        void CloseAtmWithdraw(int id);

        void DeleteAtmWithdraw(int id);

        void ChangeDebitStatus(int id, bool debitStatus);
    }
}