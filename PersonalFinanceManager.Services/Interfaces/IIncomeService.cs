using System.Collections.Generic;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        void CreateIncome(IncomeEditModel incomeEditModel);

        IList<IncomeListModel> GetIncomes(int accountId);

        IncomeEditModel GetById(int id);

        void EditIncome(IncomeEditModel incomeEditModel);

        void DeleteIncome(int id);
    }
}