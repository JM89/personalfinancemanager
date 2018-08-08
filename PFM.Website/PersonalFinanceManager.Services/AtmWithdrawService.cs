using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;
using System;

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
             
        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            throw new NotImplementedException();
        }

        public void CreateAtmWithdraws(List<AtmWithdrawEditModel> atmWithdrawEditModel)
        {
            throw new NotImplementedException();
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            throw new NotImplementedException();
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            throw new NotImplementedException();
        }
        
        public void CloseAtmWithdraw(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAtmWithdraw(int id)
        {
            throw new NotImplementedException();
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            throw new NotImplementedException();
        }
    }
}