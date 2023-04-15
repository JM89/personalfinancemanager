using System.Collections.Generic;
using PFM.Api.Contracts.AtmWithdraw;

namespace PFM.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        void CreateAtmWithdraws(List<AtmWithdrawDetails> atmWithdrawDetails);

        IList<AtmWithdrawList> GetAtmWithdrawsByAccountId(int accountId);

        void CreateAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails);

        AtmWithdrawDetails GetById(int id);

        void EditAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails);

        void CloseAtmWithdraw(int id);

        void DeleteAtmWithdraw(int id);

        void ChangeDebitStatus(int id, bool debitStatus);
    }
}