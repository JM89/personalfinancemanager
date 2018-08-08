using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Pension;
using System.Collections.Generic;
using System;

namespace PersonalFinanceManager.Services
{
    public class PensionService : IPensionService
    {
        public IList<PensionListModel> GetPensions(string userId)
        {
            throw new NotImplementedException();
        }

        public void CreatePension(PensionEditModel pensionEditModel)
        {
            throw new NotImplementedException();
        }

        public PensionEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditPension(PensionEditModel pensionEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeletePension(int id)
        {
            throw new NotImplementedException();
        }
    }
}