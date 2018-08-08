using System.Collections.Generic;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Interfaces;
using System;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IIncomeService
    {
        public void CreateIncomes(List<IncomeEditModel> incomeEditModel)
        {
            throw new NotImplementedException();
        }

        public void CreateIncome(IncomeEditModel incomeEditModel)
        {
            throw new NotImplementedException();
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            throw new NotImplementedException();
        }

        public IncomeEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditIncome(IncomeEditModel incomeEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteIncome(int id)
        {
            throw new NotImplementedException();
        }
    }
}