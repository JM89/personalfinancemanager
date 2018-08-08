using System;
using System.Collections.Generic;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Saving;

namespace PersonalFinanceManager.Services
{
    public class SavingService : ISavingService
    {
        public void CreateSaving(SavingEditModel savingEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteSaving(int id)
        {
            throw new NotImplementedException();
        }

        public void EditSaving(SavingEditModel savingEditModel)
        {
            throw new NotImplementedException();
        }

        public SavingEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<SavingListModel> GetSavingsByAccountId(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}