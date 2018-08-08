using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureTypeService : IExpenditureTypeService
    {
        public IList<ExpenditureTypeListModel> GetExpenditureTypes()
        {
            throw new NotImplementedException();
        }

        public ExpenditureTypeEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            throw new NotImplementedException();
        }

        public void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpenditureType(int id)
        {
            throw new NotImplementedException();
        }
    }
}