using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankService : IBankService
    {
        public IList<BankListModel> GetBanks()
        {
            throw new NotImplementedException();
        }

        public void Validate(BankEditModel bankEditModel)
        {
            throw new NotImplementedException();
        }

        public void CreateBank(BankEditModel bankEditModel)
        {
            throw new NotImplementedException();
        }

        public BankEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditBank(BankEditModel bankEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteBank(int id)
        {
            throw new NotImplementedException();
        }
    }
}